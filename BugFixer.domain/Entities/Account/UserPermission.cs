using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.Entities.Account
{
    public class UserPermission
    {
        #region Properties

        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }

        public long PermissionId { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }

        public Permission Permission { get; set; }

        #endregion
    }

}
