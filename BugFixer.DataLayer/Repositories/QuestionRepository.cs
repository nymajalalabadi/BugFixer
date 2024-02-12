using BugFixer.DataLayer.Context;
using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.Tags;
using BugFixer.domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.DataLayer.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        #region constractor

        private readonly BugFixerDbContext _context;

        public QuestionRepository(BugFixerDbContext context)
        {
            _context = context;
        }

        #endregion

        #region tags

        public async Task<List<Tag>> GetAllTages()
        {
            return await _context.Tags.Where(t => !t.IsDelete).ToListAsync();
        }

        public async Task<bool> IsExistsTagByName(string name)
        {
            return await _context.Tags.AnyAsync(t => t.Title.Equals(name) && !t.IsDelete);
        }

        public async Task<bool> CheckUserRequestedForTag(long userId, string tag)
        {
            return await _context.RequestTags.AnyAsync(t => t.UserId == userId && t.Title.Equals(tag) && !t.IsDelete);
        }

        public async Task AddRequestTag(RequestTag requestTag)
        {
            await _context.RequestTags.AddAsync(requestTag);    
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> RequestCountForTag(string tag)
        {
            return await _context.RequestTags.CountAsync(t => t.Title.Equals(tag) && !t.IsDelete);
        }

        public async Task AddTag(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public async Task<Tag?> GetTagByName(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => !t.IsDelete && t.Title.Equals(name));
        }

        public async Task<IQueryable<Tag>> GetAllTagsQueryable()
        {
            return _context.Tags.Where(t => !t.IsDelete).AsQueryable();
        }

        public async Task UpdateTag(Tag tag)
        {
            _context.Tags.Update(tag);
        }

        #endregion


        #region quetion

        public async Task AddQuestion(Question question)
        {
            await _context.Questions.AddAsync(question);
        }

        public async Task<IQueryable<Question>> GetAllQuestions()
        {
            return _context.Questions.Where(q => !q.IsDelete).AsQueryable();
        }

        public async Task<List<string>> GetTagListForQuestionId(long quetionsId)
        {
            return await _context.SelectQuestionTags.Include(s => s.Tag).Where(s => s.QuestionId == quetionsId).Select(s => s.Tag.Title).ToListAsync();
        }

        #endregion


        #region Select QuestionTag

        public async Task AddSelectQuestionTag(SelectQuestionTag selectQuestionTag)
        {
            await _context.SelectQuestionTags.AddAsync(selectQuestionTag);
        }

        public async Task<Question?> GetQuestionById(long id)
        {
            return await _context.Questions
                .Include(q => q.User)
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id && !q.IsDelete);
        }
        #endregion


        #region answer

        public async Task AddAnswer(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
        }

        public async Task<List<Answer>> GetAllQuestionAnswers(long questionId)
        {
            return await _context.Answers
               .Include(s => s.User)
               .Where(s => s.QuestionId == questionId && !s.IsDelete)
               .OrderByDescending(s => s.CreateDate).ToListAsync();
        }

        #endregion
    }
}
