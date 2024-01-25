using BugFixer.domain.Entities.Account;
using BugFixer.domain.Entities.Common;
using BugFixer.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.Entities.Questions
{
    public class QuestionUserScore : BaseEntity
    {
        #region Properties

        public long UserId { get; set; }

        public long QuestionId { get; set; }

        public QuestionScoreType Type { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }

        public Question Question { get; set; }

        #endregion
    }

}
