using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Admin.User
{
    public class EditUserAdminViewModel
    {
        public long UserId { get; set; }

        [Display(Name = "نام")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? LastName { get; set; }

        [Display(Name = "شماره تماس")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد .")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Password { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "تاریخ تولد")]
        [RegularExpression(@"^\d{4}/((0[1-9])|(1[012]))/((0[1-9]|[12]\d)|3[01])$", ErrorMessage = "تاریخ وارد شده معتبر نمی باشد .")]
        public string? BirthDate { get; set; }

        [Display(Name = "کشور")]
        public long? CountryId { get; set; }

        [Display(Name = "شهر")]
        public long? CityId { get; set; }

        public bool GetNewsLetter { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsBan { get; set; }

        public string Avatar { get; set; }
    }

    public enum EditUserAdminResult
    {
        Success,
        NotValidEmail,
        UserNotFound
    }
}
