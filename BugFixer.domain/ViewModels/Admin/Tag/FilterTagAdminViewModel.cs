using BugFixer.domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Admin.Tag
{
    public class FilterTagAdminViewModel : Paging<domain.Entities.Tags.Tag>
    {
        public FilterTagAdminViewModel()
        {
            Status = FilterTagAdminStatus.All;
        }

        public string? Title { get; set; }

        public FilterTagAdminStatus Status { get; set; }
    }

    public enum FilterTagAdminStatus
    {
        [Display(Name = "همه")] 
        All,

        [Display(Name = "دارای توضیحات")]
        HasDescription,

        [Display(Name = "بدون توضیحات")]
        NoDescription
    }
}
