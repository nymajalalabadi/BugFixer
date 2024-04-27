using BugFixer.Application.Extensions;
using BugFixer.Application.Generators;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.Statics;
using BugFixer.domain.Entities.Account;
using BugFixer.domain.Enums;
using BugFixer.domain.InterFaces;
using BugFixer.domain.ViewModels.Account;
using BugFixer.domain.ViewModels.Admin.User;
using BugFixer.domain.ViewModels.Common;
using BugFixer.domain.ViewModels.UserPanel.Account;
using Microsoft.Extensions.Options;

namespace BugFixer.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region constractor

        private readonly IUserRepository _userRepository;

        private IEmailService _emailService;

        private ScoreManagementViewModel _scoreManagement;

        public UserService(IUserRepository userRepository, IEmailService emailService, IOptions<ScoreManagementViewModel> scoreManagement)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _scoreManagement = scoreManagement.Value;
        }

        #endregion

        #region register

        public async Task<RegisterResult> RegisterUser(RegisterViewModel register)
        {
            if (await _userRepository.IsExistUserByEmail(register.Email.Trim().ToLower()))
            {
                return RegisterResult.EmailExists;
            }

            var user = new User()
            {
                Avatar = PathTools.DefaultUserAvatar,
                Email = register.Email.SanitizeText().Trim().ToLower(),
                Password = PasswordHelper.EncodePasswordMd5(register.Password.SanitizeText()),
                EmailActivationCode = CodeGenerator.CreateActivationCode()
            };

            await _userRepository.CreateUser(user);
            await _userRepository.SaveChanges();

            #region Send Activation Email

            var body = $@"
                <div> برای فعالسازی حساب کاربری خود روی لینک زیر کلیک کنید . </div>
                <a href='{PathTools.SiteAddress}/Activate-Email/{user.EmailActivationCode}'>فعالسازی حساب کاربری</a>
                ";

            _emailService.SendEmail(user.Email, "فعالسازی حساب کاربری", body);

            #endregion

            return RegisterResult.Success;
        }

        #endregion

        #region login

        public async Task<LoginResult> CheckUserForLogin(LoginViewModel login)
        {
            var user = await _userRepository.GetUserByEmail(login.Email.Trim().ToLower().SanitizeText());

            if (user == null)
            {
                return LoginResult.UserNotFound;
            }

            var hashPassword = PasswordHelper.EncodePasswordMd5(login.Password.SanitizeText());

            if (hashPassword != user.Password)
            {
                return LoginResult.UserNotFound;
            }

            if (user.IsDelete) return LoginResult.UserNotFound;


            if (user.IsBan) return LoginResult.UserIsBan;

            if (!user.IsEmailConfirmed) return LoginResult.EmailNotActivated;

            return LoginResult.Success;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        #endregion

        #region email activaiton 

        public async Task<bool> ActivateUserEmail(string activationCode)
        {
            var user = await _userRepository.GetUserByActivationCode(activationCode);

            if (user == null)
            {
                return false;
            }

            if (user.IsBan || user.IsDelete)
            {
                return false;
            }

            user.IsEmailConfirmed = true;
            user.EmailActivationCode = CodeGenerator.CreateActivationCode();

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            return true;
        }

        #endregion

        #region Forgot Password

        public async Task<ForgotPasswordResult> ForgotPasswordForUser(ForgotPasswordViewModel forgotPassword)
        {
            var email = forgotPassword.Email.Trim().ToLower().SanitizeText();

            var user = await _userRepository.GetUserByEmail(email);

            if (user == null || user.IsDelete) return ForgotPasswordResult.UserNotFound;

            if (user.IsBan) return ForgotPasswordResult.UserBan;

            #region Send Email Code

            var body = $@"
                <div> برای فراموشی کلمه عبور روی لینک زیر کلیک کنید . </div>
                <a href='{PathTools.SiteAddress}/Reset-Password/{user.EmailActivationCode}'>فراموشی کلمه عبور</a>
                ";

            _emailService.SendEmail(user.Email, "فراموشی کلمه عبور", body);

            #endregion

            return ForgotPasswordResult.Success;
        }

        #endregion

        #region reset password

        public async Task<ResetPasswordResult> ResetPasswordForUser(ResetPasswordViewModel resetPassword)
        {
            var user = await _userRepository.GetUserByEmail(resetPassword.EmailActivationCode.SanitizeText());

            if (user == null || user.IsDelete) return ResetPasswordResult.UserNotFound;

            if (user.IsBan) return ResetPasswordResult.UserIsBan;

            var password = PasswordHelper.EncodePasswordMd5(resetPassword.Password.SanitizeText());

            user.Password = password;
            user.IsEmailConfirmed = true;
            user.EmailActivationCode = CodeGenerator.CreateActivationCode();

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            return ResetPasswordResult.Success;
        }

        public async Task<User> GetUserByActivationCode(string activationCode)
        {
            return await _userRepository.GetUserByActivationCode(activationCode);
        }
        #endregion

        #region user panel

        public async Task<User?> GetUserById(long userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task ChangeAvatarForUser(long userId, string FileName)
        {
            var user = await _userRepository.GetUserById(userId);

            #region Delelte Avatar

            if (user.Avatar != PathTools.DefaultUserAvatar)
            {
                user.Avatar.DeleteFile(PathTools.UserAvatarServerPath);
                user.Avatar.DeleteFile(PathTools.UserAvatarServerThumb);
            }

            #endregion

            user.Avatar = FileName;

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();
        }

        public async Task<EditUserViewModel> FillEditUserViewModel(long userId)
        {
            var user = await GetUserById(userId);

            var result = new EditUserViewModel
            {
                BirthDate = user.BirthDate != null ? user.BirthDate.Value.ToShamsiDate() : string.Empty,
                CityId = user.CityId,
                CountryId = user.CountryId,
                Description = user.Description,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                GetNewsLetter = user.GetNewsLetter,
                PhoneNumber = user.PhoneNumber,
            };
            return result;
        }

        public async Task<EditUserInfoResult> EditUserInfo(EditUserViewModel editUserViewModel, long userId)
        {
            var user = await GetUserById(userId);

            if (!string.IsNullOrEmpty(editUserViewModel.BirthDate))
            {
                try
                {
                    var date = editUserViewModel.BirthDate.ToMiladi();

                    user.BirthDate = date;
                }
                catch (Exception exception)
                {
                    return EditUserInfoResult.NotValidDate;
                }
            }

            user.FirstName = editUserViewModel.FirstName.SanitizeText();
            user.LastName = editUserViewModel.LastName.SanitizeText();
            user.Description = editUserViewModel.Description.SanitizeText();
            user.PhoneNumber = editUserViewModel.PhoneNumber.SanitizeText();
            user.GetNewsLetter = editUserViewModel.GetNewsLetter;
            user.CountryId = Convert.ToInt32(editUserViewModel.CountryId);
            user.CityId = Convert.ToInt32(editUserViewModel.CityId);

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            return EditUserInfoResult.Success;
        }

        public async Task<ChangeUserPasswordResult> ChangePasswordForUser(long userId, ChnageUserPasswordViewModel changePassword)
        {
            var user = await GetUserById(userId);

            var password = PasswordHelper.EncodePasswordMd5(changePassword.OldPassword.SanitizeText());

            if (password != user.Password)
            {
                return ChangeUserPasswordResult.OldPasswordIsNotValid;
            }

            user.Password = PasswordHelper.EncodePasswordMd5(changePassword.Password.SanitizeText());

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            return ChangeUserPasswordResult.Success;
        }

        #endregion


        #region user quetion

        public async Task UpdateUserScoreAndMedal(long userId, int score)
        {
            var user = await GetUserById(userId);

            if (user == null) return;

            user.Score += score;

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            if (user.Score >= _scoreManagement.MinScoreForBronzeMedal && user.Score < _scoreManagement.MinScoreForSilverMedal)
            {
                if (user.Medal != null && user.Medal == UserMedal.Bronze)
                {
                    return;
                }

                user.Medal = UserMedal.Bronze;

                _userRepository.UpdateUser(user);
                await _userRepository.SaveChanges();
            }

            else if (user.Score >= _scoreManagement.MinScoreForSilverMedal && user.Score < _scoreManagement.MinScoreForGoldMedal)
            {
                if (user.Medal != null && user.Medal == UserMedal.Silver)
                {
                    return;
                }

                user.Medal = UserMedal.Silver;

                _userRepository.UpdateUser(user);
                await _userRepository.SaveChanges();
            }

            else if (user.Score >= _scoreManagement.MinScoreForGoldMedal)
            {
                if (user.Medal != null && user.Medal == UserMedal.Gold)
                {
                    return;
                }

                user.Medal = UserMedal.Gold;

                _userRepository.UpdateUser(user);
                await _userRepository.SaveChanges();
            }
        }

        #endregion

        #region admin

        public async Task<FilterUserAdminViewModel> FilterUserAdmin(FilterUserAdminViewModel filter)
        {
            var query = _userRepository.GetAllUsers();

            #region Filter

            if (!string.IsNullOrEmpty(filter.UserSearch))
            {
                query = query.Where(s => (s.FirstName + " " + s.LastName).Trim().Contains(filter.UserSearch) || s.Email.Contains(filter.UserSearch));
            }

            #endregion

            #region Status

            switch (filter.ActivationStatus)
            {
                case AccountActivationStatus.All:
                    break;
                case AccountActivationStatus.IsActive:
                    query = query.Where(u => u.IsEmailConfirmed);
                    break;
                case AccountActivationStatus.NotActive:
                    query = query.Where(u => !u.IsEmailConfirmed);
                    break;
            }

            #endregion

            await filter.SetPaging(query);

            return filter;
        }

        public async Task<EditUserAdminViewModel?> FillEditUserAdminViewModel(long userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return null;
            }

            return new EditUserAdminViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,
                BirthDate = user.BirthDate?.ToShamsiDate(),
                CountryId = user.CountryId,
                CityId = user.CityId,
                GetNewsLetter = user.GetNewsLetter,
                IsAdmin = user.IsAdmin,
                IsBan = user.IsBan,
                Avatar = user.Avatar,
                IsEmailConfirmed = user.IsEmailConfirmed,
                LastName = user.LastName,
                UserId = user.Id
            };
        }

        public async Task<EditUserAdminResult> EditUserAdmin(EditUserAdminViewModel editUser)
        {
            var user = await _userRepository.GetUserById(editUser.UserId);

            if (user == null)
            {
                return EditUserAdminResult.UserNotFound;
            }

            if (!user.Email.Equals(editUser.Email) && await _userRepository.IsExistUserByEmail(editUser.Email))
            {
                return EditUserAdminResult.NotValidEmail;
            }

            user.Email = editUser.Email;
            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            user.Description = editUser.Description;
            user.IsBan = editUser.IsBan;
            user.IsEmailConfirmed = editUser.IsEmailConfirmed;
            user.GetNewsLetter = editUser.GetNewsLetter;
            user.IsAdmin = editUser.IsAdmin;
            user.BirthDate = editUser.BirthDate?.ToMiladi();
            user.PhoneNumber = editUser.PhoneNumber;
            user.CountryId = Convert.ToInt32(editUser.CountryId);
            user.CityId = Convert.ToInt32(editUser.CityId);

            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = editUser.Password;
            }

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            return EditUserAdminResult.Success;
        }

        #endregion
    }
}
