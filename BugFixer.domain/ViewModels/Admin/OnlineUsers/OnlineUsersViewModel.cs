using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.ViewModels.Admin.OnlineUsers
{
    public class OnlineUsersViewModel
    {
        public string UserId { get; set; }

        public string DisplayName { get; set; }

        public string ConnectedDate { get; set; }
    }
}
