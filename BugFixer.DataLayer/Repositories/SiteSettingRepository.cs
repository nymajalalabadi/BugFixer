using BugFixer.DataLayer.Context;
using BugFixer.domain.Entities.SiteSetting;
using BugFixer.domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.DataLayer.Repositories
{
    public class SiteSettingRepository : ISiteSettingRepository
    {
        #region constractor

        private readonly BugFixerDbContext _context;

        public SiteSettingRepository(BugFixerDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<EmailSetting> GetDefaultEmail()
        {
            return await _context.EmailSettings.FirstOrDefaultAsync(e => !e.IsDefault && e.IsDefault);
        }

    }
}
