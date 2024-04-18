using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Question;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.Admin.Controllers
{
    public class QuestionController : AdminBaseController
    {
        #region Consractor

        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        public async Task<IActionResult> QuestionsList(FilterQuestionViewModel filter)
        {
            var result = await _questionService.FilterQuestion(filter);

            return View(result);
        }

    }
}
