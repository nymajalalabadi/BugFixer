using BugFixer.Application.Services.Implementations;
using BugFixer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.ViewComponents
{
    public class QuestionAnswersListViewComponent : ViewComponent
    {
        #region constractor

        private readonly IQuestionService _questionService;

        public QuestionAnswersListViewComponent(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(long questionId)
        {
            var answers = await _questionService.GetAllQuestionAnswers(questionId);

            return View("QuestionAnswersList", answers);
        }
    }
}
