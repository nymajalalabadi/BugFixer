using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Controllers
{
    public class QuestionController : BaseController
    {
        #region constractor

        public QuestionController()
        {
            
        }

        #endregion

        #region Create Question

        [Authorize]
        [HttpGet("create-question")]
        public async Task<IActionResult> CreateQuestion()
        {
            return View();
        }

        #endregion

    }
}
