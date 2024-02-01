using BugFixer.Application.Statics;
using Microsoft.AspNetCore.Mvc;
using BugFixer.Application.Extensions;

namespace BugFixer.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
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