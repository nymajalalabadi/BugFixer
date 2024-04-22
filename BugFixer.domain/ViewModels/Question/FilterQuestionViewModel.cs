using BugFixer.domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Question
{
    public class FilterQuestionViewModel : Paging<QuestionListViewModel>
    {
        public FilterQuestionViewModel()
        {
            Sort = FilterQuestionSortEnum.NewToOld;
            CheckedStatus = FilterQuestionCheckedStatus.All;
        }

        public string? Title { get; set; }

        public string? TagTitle { get; set; }

        public FilterQuestionSortEnum Sort { get; set; }

        public FilterQuestionCheckedStatus CheckedStatus { get; set; }
    }

    public enum FilterQuestionSortEnum
    {
        [Display(Name = "تاریخ ثبت نزولی")] 
        NewToOld,

        [Display(Name = "تاریخ ثبت صعودی")] 
        OldToNew,

        [Display(Name = "امتیاز نزولی")] 
        ScoreHighToLow,

        [Display(Name = "امتیاز صعودی")] 
        ScoreLowToHigh
    }

    public enum FilterQuestionCheckedStatus
    {
        [Display(Name = "همه")] 
        All,

        [Display(Name = "بررسی شده")] 
        IsChecked,

        [Display(Name = "بررسی نشده")] 
        NotChecked,
    }

}
