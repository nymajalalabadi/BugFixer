using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Question;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.ViewComponents
{
    #region Score Des Questions

    public class ScoreDesQuestionsViewComponent : ViewComponent
    {
        #region Constractor

        private IQuestionService _questionService;

        public ScoreDesQuestionsViewComponent(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var options = new FilterQuestionViewModel
            {
                TakeEntity = 5,
                Sort = FilterQuestionSortEnum.ScoreHighToLow
            };

            var result = await _questionService.FilterQuestion(options);

            return View("ScoreDesQuestions", result);
        }
    }

    #endregion

    #region Create Date Des Questions

    public class CreateDateDesQuestionsViewComponent : ViewComponent
    {
        #region Constractor

        private IQuestionService _questionService;

        public CreateDateDesQuestionsViewComponent(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var options = new FilterQuestionViewModel
            {
                TakeEntity = 5,
                Sort = FilterQuestionSortEnum.NewToOld
            };

            var result = await _questionService.FilterQuestion(options);

            return View("CreateDateDesQuestions", result);
        }
    }

    #endregion

    #region Use Count Des Tags

    public class UseCountDesTagsViewComponent : ViewComponent
    {
        #region Constractor

        private IQuestionService _questionService;

        public UseCountDesTagsViewComponent(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var options = new FilterTagViewModel
            {
                TakeEntity = 10,
                Sort = FilterTagEnum.UseCountHighToLow
            };

            var result = await _questionService.FilterTags(options);

            return View("UseCountDesTags", result);
        }
    }

    #endregion
}
