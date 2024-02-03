using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Question
{
    public class QuestionListViewModel
    {
        public long QuestionId { get; set; }

        public string Title { get; set; }

        public string UserDisplayName { get; set; }

        public string CreateDate { get; set; }

        public bool HasAnyAnswer { get; set; }

        public bool HasAnyTrueAnswer { get; set; }

        public int AnswersCount { get; set; }

        public int Score { get; set; }

        public int ViewCount { get; set; }

        public string? AnswerByDisplayName { get; set; }

        public string? AnswerByCreateDate { get; set; }

        public List<string> Tags { get; set; }
    }

}
