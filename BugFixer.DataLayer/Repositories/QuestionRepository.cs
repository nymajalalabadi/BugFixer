using BugFixer.DataLayer.Context;
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

        public async Task<List<Tag>> GetAllTages()
        {
            return await _context.Tags.Where(t => !t.IsDelete).ToListAsync();
        }

    }
}
