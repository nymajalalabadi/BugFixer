using BugFixer.domain.Entities.Account;
using BugFixer.domain.ViewModels.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Interfaces
{
    public interface IUserService
    {
        #region register

        Task<RegisterResult> RegisterUser(RegisterViewModel register);

        #endregion

        #region login

        Task<LoginResult> CheckUserForLogin(LoginViewModel login);

        Task<User> GetUserByEmail(string email);

        #endregion

        #region email activaiton 

        Task<bool> ActivateUserEmail(string activationCode);

        #endregion

        #region Forgot Password

        Task<ForgotPasswordResult> ForgotPasswordForUser(ForgotPasswordViewModel forgotPassword);

        #endregion

        #region Reset Password

        Task<ResetPasswordResult> ResetPasswordForUser(ResetPasswordViewModel resetPassword);

        Task<User> GetUserByActivationCode(string activationCode);

        #endregion

        #region user panel

        Task<User?> GetUserById(long userId);

        Task ChangeAvatarForUser(long userId, string FileName);
        

        #endregion
    }
}
