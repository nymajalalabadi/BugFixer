using BugFixer.domain.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.InterFaces
{
    public interface IStateRepository
    {
        Task<List<State>> GetAllStates(long? stateId = null);
    }
}
