using BugFixer.domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.DataLayer.Context
{
    public class BugFixerDbContext : DbContext
    {
        public BugFixerDbContext(DbContextOptions<BugFixerDbContext> options) : base(options) { }

        #region Tables

        public DbSet<User> Users { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
