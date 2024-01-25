using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.Enums
{
    public enum UserMedal
    {
        [Display(Name = "برنز")] 
        Bronze,

        [Display(Name = "نقره")] 
        Silver,

        [Display(Name = "طلا")] 
        Gold
    }
}
