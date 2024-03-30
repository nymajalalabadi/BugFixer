using BugFixer.domain.Entities.Questions;
using BugFixer.domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.UserPanel.Question
{
    public class FilterQuestionBookmarksViewModel : Paging<Entities.Questions.Question>
    {
        public long UserId { get; set; }
    }
}
