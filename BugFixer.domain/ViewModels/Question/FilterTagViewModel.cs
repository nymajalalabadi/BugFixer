using BugFixer.domain.Entities.Tags;
using BugFixer.domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Question
{
    public class FilterTagViewModel : Paging<Tag>
    {
        public string? Title { get; set; }

        public FilterTagEnum Sort { get; set; }
    }

    public enum FilterTagEnum
    {
        [Display(Name = "تاریخ ثبت نزولی")] NewToOld,
        [Display(Name = "تاریخ ثبت صعودی")] OldToNew,
        [Display(Name = "تعداد استفاده نزولی")] UseCountHighToLow,
        [Display(Name = "تعداد استفاده صعودی")] UseCountLowToHigh
    }

}
