using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.Tags;
using BugFixer.domain.Enums;
using BugFixer.domain.ViewModels.Admin.Tag;
using BugFixer.domain.ViewModels.Question;
using BugFixer.domain.ViewModels.UserPanel.Question;

namespace BugFixer.Application.Services.Interfaces
{
    public interface IQuestionService
    {
        #region tag

        Task<List<Tag>> GetAllTages();

        Task<CreateQuestionResult> CheckTagValidation(List<string>? tags, long userId);

        #endregion

        #region quetion

        Task<IQueryable<Question>> GetAllQuestions();

        Task<bool> CreateQuetion(CreateQuestionViewModel createQuestion);

        Task<bool> EditQuetion(EditQuestionViewModel editQuestion);

        Task<FilterQuestionViewModel> FilterQuestion(FilterQuestionViewModel filterQuestion);

        Task<FilterQuestionBookmarksViewModel> FilterQuestionBookmarks(FilterQuestionBookmarksViewModel filterQuestion);

        Task<FilterTagViewModel> FilterTags(FilterTagViewModel filterTags);

        Task<Question?> GetQuestionById(long id);

        Task<List<string>> GetTagListForQuestionId(long quetionsId);

        Task<bool> AnswerQuestion(AnswerQuestionViewModel answerQuestion);

        Task AddViewForQuestion(string userIp, Question question);

        Task<bool> AddQuestionToBookmark(long questionId, long userId);

        Task<bool> IsExistsQuestionInUserBookmarks(long questionId, long userId);

        Task<EditQuestionViewModel?> FillEditQuestionViewModel(long questionId, long userId);

        #endregion


        #region Answer

        Task<List<Answer>> GetAllQuestionAnswers(long questionId);

        Task<bool> HasUserAccessToSelectTrueAnswer(long userId, long answerId);

        Task SelectTrueAnswer(long userId, long answerId);

        Task<CreateScoreForAnswerResult> CreateScoreForAnswer(long asnwerId, AnswerScoreType type, long userId);

        Task<CreateScoreForQuestionResult> CreateScoreForQuestion(long questionId, QuestionScoreType type, long userId);

        Task<EditAnswerViewModel?> FillEditAnswerViewModel(long answerId, long userId);

        Task<bool> EditAnswer(EditAnswerViewModel editAnswerViewModel);

        #endregion


        #region Admin

        Task<List<TagViewModelJson>> GetTagViewModelJson();

        #endregion
    }
}
