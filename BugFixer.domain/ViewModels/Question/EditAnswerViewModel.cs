using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Question
{
    public class EditAnswerViewModel
    {
        public long AnswerId { get; set; }

        public string Answer { get; set; }

        public long QuestionId { get; set; }

        public long UserId { get; set; }
    }
}
