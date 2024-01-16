using BugFixer.Application.Generators;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.Statics;
using BugFixer.domain.Entities.Account;
using BugFixer.domain.InterFaces;
using BugFixer.domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region constractor

        private readonly IUserRepository _userRepository;

        private IEmailService _emailService;

        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;

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

        public async Task<ForgotPasswordResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            var email = forgotPassword.Email.Trim().ToLower().SanitizeText();

            var user = await _userRepository.GetUserByEmail(email);

            if (user == null || user.IsDelete) return ForgotPasswordResult.UserNotFound;

            if (user.IsBan) return ForgotPasswordResult.UserBan;

            #region Send Activation Email

            var body = $@"
                <div> برای فراموشی کلمه عبور روی لینک زیر کلیک کنید . </div>
                <a href='{PathTools.SiteAddress}/Reset-Password/{user.EmailActivationCode}'>فراموشی کلمه عبور</a>
                ";

            _emailService.SendEmail(user.Email, "فراموشی کلمه عبور", body);

            #endregion

            return ForgotPasswordResult.Success;
        }

        #endregion
    }
}
