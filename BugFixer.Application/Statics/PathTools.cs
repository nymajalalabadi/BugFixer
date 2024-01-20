using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Statics
{
    public class PathTools
    {
        #region User

        public static readonly string DefaultUserAvatar = "DefaultAvatar.png";

        public static readonly string UserAvatarServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwrroot/content/user/");
        public static readonly string UserAvatarPath = "/content/user/";

        #endregion

        #region site

        public static readonly string SiteAddress = "https://localhost:7109";

        #endregion
    }
}
