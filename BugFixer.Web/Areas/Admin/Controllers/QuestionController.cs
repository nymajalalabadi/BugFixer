using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Question;
using BugFixer.Web.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.Admin.Controllers
{
    [PermissionChecker(3)]
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

        #region Delete Question

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(long Id)
        {
            var result = await _questionService.DeleteQuestion(Id);

            if (!result)
            {
                return new JsonResult(new { status = "error", message = "مقادیر ورودی معتبر نمی باشد." });
            }

            return new JsonResult(new { status = "success", message = "عملیات با موفقیت انجام شد." });
        }

        #endregion


        #region Is Checked Question

        [HttpPost]
        public async Task<IActionResult> ChangeIsCheckedQuestion(long id)
        {
            var result = await _questionService.ChangeQuestionIsCheck(id);

            if (!result)
            {
                return new JsonResult(new { status = "error", message = "مقادیر ورودی معتبر نمی باشد." });
            }

            return new JsonResult(new { status = "success", message = "عملیات با موفقیت انجام شد." });

        }

        #endregion
    }
}
