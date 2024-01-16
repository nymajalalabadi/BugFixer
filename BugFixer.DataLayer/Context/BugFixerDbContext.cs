using BugFixer.domain.Entities.Account;
using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.SiteSetting;
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

        public DbSet<EmailSetting> EmailSettings { get; set; }

        public DbSet<Question> Questions { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seed Data

            var date = DateTime.MinValue;

            modelBuilder.Entity<EmailSetting>().HasData(new EmailSetting()
            {
                CreateDate = date,
                DisplayName = "BugFixer",
                EnableSSL = true,
                From = "nymasteam@gmail.com",
                Id = 1,
                IsDefault = true,
                IsDelete = false,
                Password = "qjymwzfmsycwpzza",
                Port = 587,
                SMTP = "smtp.gmail.com"
            });

            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
