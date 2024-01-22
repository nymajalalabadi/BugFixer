using BugFixer.domain.Entities.Common;
using BugFixer.domain.Entities.Location;
using BugFixer.domain.Entities.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.Entities.Account
{
    public class User : BaseEntity
    {
        #region Properties

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
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? CountryId { get; set; }

        public int? CityId { get; set; }

        public bool GetNewsLetter { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsBan { get; set; }

        public string EmailActivationCode { get; set; }

        public string Avatar { get; set; }

        #endregion


        #region Relations

        [InverseProperty("UserCountries")]
        public State? Country { get; set; }

        [InverseProperty("UserCities")]
        public State? City { get; set; }

        public ICollection<Question> Questions { get; set; }

        #endregion
    }
}
