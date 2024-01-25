using BugFixer.domain.Entities.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.Entities.Account
{
    public class UserQuestionBookmark
    {
        #region Properties

        [Key]
        public long Id { get; set; }

        public long QuestionId { get; set; }

        public long UserId { get; set; }

        #endregion

        #region Relations

        public Question Question { get; set; }

        public User User { get; set; }

        #endregion
    }

}
