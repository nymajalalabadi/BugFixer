using BugFixer.domain.Entities.Account;
using BugFixer.domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.Entities.Questions
{
    public class Question : BaseEntity
    {
        #region Properties

        [Display(Name = "عنوان")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        public long UserId { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Content { get; set; }

        public bool IsChecked { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int ViewCount { get; set; } = 0;

        [Display(Name = "امتیاز")]
        public int Score { get; set; } = 0;

        #endregion

        #region Relations

        public User User { get; set; }

        public ICollection<SelectQuestionTag> SelectQuestionTags { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public ICollection<QuestionView> QuestionViews { get; set; }

        public ICollection<UserQuestionBookmark> UserQuestionBookmarks { get; set; }

        public ICollection<QuestionUserScore> QuestionUserScores { get; set; }

        #endregion

    }
}
