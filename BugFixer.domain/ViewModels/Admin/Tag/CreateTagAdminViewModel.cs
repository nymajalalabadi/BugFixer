using System.ComponentModel.DataAnnotations;

namespace BugFixer.domain.ViewModels.Admin.Tag
{
    public class CreateTagAdminViewModel
    {
        [Display(Name = "عنوان")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }
}
