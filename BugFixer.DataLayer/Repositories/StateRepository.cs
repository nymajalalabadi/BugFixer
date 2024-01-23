using BugFixer.DataLayer.Context;
using BugFixer.domain.Entities.Location;
using BugFixer.domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.DataLayer.Repositories
{
    public class StateRepository : IStateRepository
    {
        #region constractor

        private readonly BugFixerDbContext _context;

        public StateRepository(BugFixerDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<List<State>> GetAllStates(long? stateId = null)
        {
            var states = _context.States.Where(s => !s.IsDelete).AsQueryable();

            if (stateId.HasValue)
            {
                states = states.Where(s => s.ParentId.HasValue && s.ParentId.Value == stateId.Value) ;
            }
            else
            {
                states = states.Where(s => s.ParentId == null);
            }

            return await states.ToListAsync();
        }

    }
}
