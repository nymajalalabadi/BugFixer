using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.Enums
{
    public enum QuestionScoreType
    {
        [Display(Name = "مثبت")] 
        Plus,

        [Display(Name = "منفی")] 
        Minus,
    }
}
