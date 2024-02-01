using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.InterFaces;
using BugFixer.domain.ViewModels.Common;

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
