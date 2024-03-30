using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.UserPanel.Question;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    public class QuestionController : UserPanelBaseController
    {
        #region consractor

        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        #region BookMarks

        [HttpGet]
        public async Task<IActionResult> QuestionBookmarks(FilterQuestionBookmarksViewModel filter)
        {
            filter.UserId = User.GetUserId();

            var result = await _questionService.FilterQuestionBookmarks(filter);

            return View(result);
        }

        #endregion
    }
}
