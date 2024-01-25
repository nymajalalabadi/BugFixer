using BugFixer.domain.Entities.Account;
using BugFixer.domain.Entities.Location;
using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.SiteSetting;
using BugFixer.domain.Entities.Tags;
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

        public DbSet<State> States { get; set; }

        public DbSet<EmailSetting> EmailSettings { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<SelectQuestionTag> SelectQuestionTags { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<RequestTag> RequestTags { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<QuestionView> QuestionViews { get; set; }

        public DbSet<UserQuestionBookmark> UserQuestionBookmarks { get; set; }

        public DbSet<QuestionUserScore> QuestionUserScores { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }

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
