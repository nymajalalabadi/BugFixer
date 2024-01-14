using BugFixer.domain.ViewModels.Account;
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
    }
}
