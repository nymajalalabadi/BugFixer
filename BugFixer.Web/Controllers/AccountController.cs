﻿using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Account;
using BugFixer.Web.ActionFilters;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugFixer.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region constractor

        private readonly IUserService _userService;

        private ICaptchaValidator _captchaValidator;

        public AccountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region login

        [HttpGet("login")]
        [RedirectHomeIfLoggedInActionFilter]
        public IActionResult Login(string ReturnUrl = "")
        {
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/");
            //}

            var result = new LoginViewModel();

            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                result.ReturnUrl = ReturnUrl;
            }

            return View(result);
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData[ErrorMessage] = "اعتبار سنجی Captcha با خطا مواجه شد لطفا مجدد تلاش کنید .";
                return View(login);
            }

            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _userService.CheckUserForLogin(login);

            switch (result)
            {
                case LoginResult.UserIsBan:
                    TempData[WarningMessage] = "دسترسی شما به سایت مسدود می باشد .";
                    break;
                case LoginResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد .";
                    break;
                case LoginResult.EmailNotActivated:
                    TempData[WarningMessage] = "برای ورود به حساب کاربری ابتدا ایمیل خود را فعال کنید .";
                    break;
                case LoginResult.Success:

                    var user = await _userService.GetUserByEmail(login.Email);

                    #region Login User

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties { IsPersistent = login.RememberMe };

                    await HttpContext.SignInAsync(principal, properties);

                    #endregion

                    TempData[SuccessMessage] = "خوش آمدید";

                    if (!string.IsNullOrEmpty(login.ReturnUrl))
                    {
                        return Redirect(login.ReturnUrl);
                    }

                    return Redirect("/");
            }

            return View(login);
        }

        #endregion

        #region register

        [HttpGet("register")]
        [RedirectHomeIfLoggedInActionFilter]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData[ErrorMessage] = "اعتبار سنجی Captcha با خطا مواجه شد لطفا مجدد تلاش کنید .";
                return View(register);
            }

            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var result = await _userService.RegisterUser(register);

            switch (result)
            {
                case RegisterResult.EmailExists:
                    TempData[ErrorMessage] = "ایمیل وارد شده از قبل موجود است";
                    break;

                case RegisterResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                    return RedirectToAction("Login","Account");
            }

            return View(register);
        }

        #endregion

        #region logout

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        #endregion

        #region email activation

        [HttpGet("Activate-Email/{activationCode}")]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ActivationUserEmail(string activationCode)
        {
            var result = await _userService.ActivateUserEmail(activationCode);

            if (result)
            {
                TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد .";
            }
            else
            {
                TempData[ErrorMessage] = "فعال سازی حساب کاربری با خطا مواجه شد .";
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region forgot password

        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgotPassword.Captcha))
            {
                TempData[ErrorMessage] = "اعتبار سنجی Captcha با خطا مواجه شد لطفا مجدد تلاش کنید .";
                return View(forgotPassword);
            }

            if (!ModelState.IsValid)
            {
                return View(forgotPassword);
            }

            var result = await _userService.ForgotPasswordForUser(forgotPassword);

            switch (result)
            {
                case ForgotPasswordResult.UserBan:
                    TempData[WarningMessage] = "دسترسی شما به حساب کاربری مسدود می باشد .";
                    break;

                case ForgotPasswordResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد .";
                    break;

                case ForgotPasswordResult.Success:
                    TempData[InfoMessage] = "لینک بازیابی رمز عبور به ایمیل شما ارسال شد .";

                    return RedirectToAction("Login", "Account");
            }

            return View(forgotPassword);
        }

        #endregion

        #region Reset-Password

        [HttpGet("Reset-Password/{emailActivationCode}")]
        public async Task<IActionResult> ResetPassword(string emailActivationCode)
        {
            var user = await _userService.GetUserByActivationCode(emailActivationCode);

            if (user == null || user.IsBan || user.IsDelete) return NotFound();

            return View(new ResetPasswordViewModel()
            {
                EmailActivationCode = user.EmailActivationCode,
            });
        }

        [HttpPost("Reset-Password/{emailActivationCode}")]
        [RedirectHomeIfLoggedInActionFilter]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(resetPassword.Captcha))
            {
                TempData[ErrorMessage] = "اعتبار سنجی Captcha با خطا مواجه شد لطفا مجدد تلاش کنید .";
                return View(resetPassword);
            }

            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            var result = await _userService.ResetPasswordForUser(resetPassword);

            switch (result)
            {
                case ResetPasswordResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد .";
                    break;

                case ResetPasswordResult.UserIsBan:
                    TempData[WarningMessage] = "دسترسی شما به سایت مسدود می باشد .";
                    break;

                case ResetPasswordResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                    return RedirectToAction("Login", "Account");
            }

            return View(resetPassword);
        }
        #endregion
    }
}
