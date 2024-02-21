using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Common
{
    public class ScoreManagementViewModel
    {
        public int MinRequestCountForVerifyTag { get; set; }

        public int AddNewQuestionScore { get; set; }

        public int AddNewAnswerScore { get; set; }

        public int MinScoreForBronzeMedal { get; set; }

        public int MinScoreForSilverMedal { get; set; }

        public int MinScoreForGoldMedal { get; set; }

        public int MinScoreForUpScoreAnswer { get; set; }

        public int MinScoreForDownScoreAnswer { get; set; }

    }
}
