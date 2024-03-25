using BugFixer.Application.Statics;
using Microsoft.AspNetCore.Mvc;
using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Question;

namespace BugFixer.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region consractor

        private readonly IQuestionService _questionService;

        public HomeController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            var options = new FilterQuestionViewModel
            {
                TakeEntity = 5,
                Sort = FilterQuestionSortEnum.NewToOld
            };

            var result = await _questionService.FilterQuestion(options);

            ViewData["Questions"] = result;

            return View();
        }

        #region Editor Upload

        public async Task<IActionResult> UploadEditorImage(IFormFile upload)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName);

            upload.UploadFile(fileName, PathTools.EditorImageServerPath);

            return Json(new { url = $"{PathTools.EditorImagePath}{fileName}" });
        }

        #endregion

    }
}