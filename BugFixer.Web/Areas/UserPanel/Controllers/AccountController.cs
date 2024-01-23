using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.UserPanel.Account;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    public class AccountController : UserPanelBaseController
    {
        #region constractor

        private readonly IStateService _stateService;

        public AccountController(IStateService stateService)
        {
                _stateService = stateService;
        }

        #endregion

        #region edit user info

        [HttpGet]
        public async Task<IActionResult> EditInfo()
        {
            ViewData["states"] = await _stateService.GetAllState();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInfo(EditUserViewModel edit)
        {
            return View();
        }

        #endregion

        #region load cities

        public async Task<IActionResult> LoadCities(long countryId)
        {
            var result = await _stateService.GetAllState(countryId);

            return new JsonResult(result); 
        }

        #endregion
    }
}
