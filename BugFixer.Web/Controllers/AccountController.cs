using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region constractor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region login

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        #endregion

        #region register

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            return View();
        }

        #endregion
    }
}
