using BugFixer.domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Account
{
    public class ForgotPasswordViewModel : GoogleRecaptchaViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد .")]
        public string Email { get; set; }
    }

    public enum ForgotPasswordResult
    {
        UserBan,
        UserNotFound,
        Success
    }
}
