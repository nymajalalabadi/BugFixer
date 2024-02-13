using BugFixer.Application.Extensions;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Question;
using BugFixer.Web.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BugFixer.Web.Controllers
{
    public class QuestionController : BaseController
    {
        #region constractor

        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        #region Create Question

        [Authorize]
        [HttpGet("create-question")]
        public async Task<IActionResult> CreateQuestion()
        {
            return View();
        }

        [Authorize]
        [HttpPost("create-question"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel createQuestion)
        {
            var tagResult = await _questionService.CheckTagValidation(createQuestion.SelectedTags, HttpContext.User.GetUserId());

            if (tagResult.Status == CreateQuestionResultEnum.NotValidTag)
            {
                createQuestion.SelectedTagsJson = JsonConvert.SerializeObject(createQuestion.SelectedTags);
                createQuestion.SelectedTags = null;

                TempData[WarningMessage] = tagResult.Message;

                return View(createQuestion);
            }

            createQuestion.UserId = User.GetUserId();

            var result = await _questionService.CreateQuetion(createQuestion);

            if (result)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                return Redirect("/");
            }

            createQuestion.SelectedTagsJson = JsonConvert.SerializeObject(createQuestion.SelectedTags);
            createQuestion.SelectedTags = null;

            return View(createQuestion);
        }

        #endregion


        #region Get Tags

        [HttpGet("get-tags")]
        public async Task<IActionResult> GetTagsForSuggest(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(null);
            }

            var tags = await _questionService.GetAllTages();

            var filteredTags = tags.Where(s => s.Title.Contains(name))
                .Select(s => s.Title)
                .ToList();

            return Json(filteredTags);
        }

        #endregion

        #region quetion list

        [HttpGet("questions")]
        public async Task<IActionResult> QuestionList(FilterQuestionViewModel filter)
        {
            var result = await _questionService.FilterQuestion(filter);

            return View(result);  
        }

        #endregion

        #region filter question by tag 

        [HttpGet("tags{tagName}")]
        public async Task<IActionResult> QuestionListByTag(FilterQuestionViewModel filter, string tagName)
        {
            tagName = tagName.Trim().ToLower().SanitizeText();

            filter.TagTitle = tagName;

            var result = await _questionService.FilterQuestion(filter);

            ViewBag.TagTitle = tagName;

            return View(result);
        }

        #endregion

        #region Filter Tags

        [HttpGet("tags")]
        public async Task<IActionResult> FilterTags(FilterTagViewModel filter)
        {
            filter.TakeEntity = 12;

            var result = await _questionService.FilterTags(filter);

            return View(result);
        }

        #endregion

        #region Quesyion Detail

        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> QuestionDetail(long questionId)
        {
            var question = await _questionService.GetQuestionById(questionId);

            if (question == null) return NotFound();

            var userIp = Request.HttpContext.Connection.RemoteIpAddress;
             
            if (userIp != null)
            {
                await _questionService.AddViewForQuestion(userIp.ToString(), question);
            }

            ViewData["TagList"] = await _questionService.GetTagListForQuestionId(questionId);

            return View(question);
        }

        #region Answer Question

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AnswerQuestion(AnswerQuestionViewModel answerQuestion)
        {
            if (string.IsNullOrEmpty(answerQuestion.Answer))
            {
                return new JsonResult(new { status = "EmptyAnswer" });
            }

            answerQuestion.UserId = User.GetUserId();

            var result = await _questionService.AnswerQuestion(answerQuestion);

            if (result)
            {
                return new JsonResult(new { status = "Success" });
            }

            return new JsonResult(new { status = "Error" });
        }

        #endregion

        #endregion



    }
}
