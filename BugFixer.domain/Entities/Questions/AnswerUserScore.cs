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
    public class AnswerUserScore : BaseEntity
    {
        #region Properties

        public long UserId { get; set; }

        public long AnswerId { get; set; }

        public AnswerScoreType Type { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }

        public Answer Answer { get; set; }

        #endregion
    }
}
