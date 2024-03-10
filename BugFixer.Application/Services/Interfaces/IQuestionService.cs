﻿using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.Tags;
using BugFixer.domain.Enums;
using BugFixer.domain.ViewModels.Question;

namespace BugFixer.Application.Services.Interfaces
{
    public interface IQuestionService
    {
        #region tag

        Task<List<Tag>> GetAllTages();

        Task<CreateQuestionResult> CheckTagValidation(List<string>? tags, long userId);

        Task<bool> CreateQuetion(CreateQuestionViewModel createQuestion);

        #endregion

        #region quetion

        Task<FilterQuestionViewModel> FilterQuestion(FilterQuestionViewModel filterQuestion);

        Task<FilterTagViewModel> FilterTags(FilterTagViewModel filterTags);

        Task<Question?> GetQuestionById(long id);

        Task<List<string>> GetTagListForQuestionId(long quetionsId);

        Task<bool> AnswerQuestion(AnswerQuestionViewModel answerQuestion);

        Task AddViewForQuestion(string userIp, Question question);

        Task<bool> AddQuestionToBookmark(long questionId, long userId);

        #endregion


        #region Answer

        Task<List<Answer>> GetAllQuestionAnswers(long questionId);

        Task<bool> HasUserAccessToSelectTrueAnswer(long userId, long answerId);

        Task SelectTrueAnswer(long userId, long answerId);

        Task<CreateScoreForAnswerResult> CreateScoreForAnswer(long asnwerId, AnswerScoreType type, long userId);

        Task<CreateScoreForQuestionResult> CreateScoreForQuestion(long questionId, QuestionScoreType type, long userId);

        #endregion

    }
}
