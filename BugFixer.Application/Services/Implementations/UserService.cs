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

            var password = PasswordHelper.EncodePasswordMd5(register.Password);

            var user = new User()
            {
                Avatar = PathTools.DefaultUserAvatar,
                Email = register.Email.Trim().ToLower(),
                Password = password,
                EmailActivationCode = CodeGenerator.CreateActivationCode()
            };

            await _userRepository.CreateUser(user);
            await _userRepository.SaveChanges();

            return RegisterResult.Success;
        }

        #endregion
    }
}
