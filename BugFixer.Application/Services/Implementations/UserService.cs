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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
