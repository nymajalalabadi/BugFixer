using BugFixer.domain.ViewModels.Common;

namespace BugFixer.Application.Services.Interfaces
{
    public interface IStateService
    {
        Task<List<SelectListViewModel>> GetAllStates(long? stateId = null);
    }
}
