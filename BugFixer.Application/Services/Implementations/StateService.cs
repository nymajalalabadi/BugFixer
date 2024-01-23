using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.Entities.Common;
using BugFixer.domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Implementations
{
    public class StateService : IStateService
    {
        #region constractor

        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        #endregion

        public async Task<List<SelectListViewModel>> GetAllStates(long? stateId = null)
        {
            var states = await _stateRepository.GetAllStates(stateId); 

            return states.Select(s => new SelectListViewModel()
            {
                Id = s.Id,
                Title = s.Title,
            }).ToList();
        }

    }
}
