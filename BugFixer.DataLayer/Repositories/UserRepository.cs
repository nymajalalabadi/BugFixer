using BugFixer.DataLayer.Context;
using BugFixer.domain.Entities.Account;
using BugFixer.domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.DataLayer.Repositories
{
    public class UserRepository : IUserRepository 
    {
        #region constractor

        private readonly BugFixerDbContext _context;

        public UserRepository(BugFixerDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<bool> IsExistUserByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

    }
}
