using BugFixer.domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Interfaces
{
    public interface IStateService
    {
        Task<List<SelectListViewModel>> GetAllStates(long? stateId = null);
    }
}
