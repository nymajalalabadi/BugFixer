using BugFixer.domain.Entities.Account;
using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.Tags;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.domain.InterFaces
{
    public interface IQuestionRepository
    {
        #region tags

        Task<List<Tag>> GetAllTages();

        Task<bool> IsExistsTagByName(string name);

        Task<bool> CheckUserRequestedForTag(long userId, string tag);

        Task AddRequestTag(RequestTag requestTag);

        Task SaveChanges();

        Task<int> RequestCountForTag(string tag);

        Task AddTag(Tag tag);

        Task<Tag?> GetTagByName(string name);

        Task<IQueryable<Tag>> GetAllTagsQueryable();

        Task UpdateTag(Tag tag);

        #endregion

        #region quetion

        Task AddQuestion(Question question);

        Task<IQueryable<Question>> GetAllQuestions();

        Task<List<string>> GetTagListForQuestionId(long quetionsId);

        Task UpdateQuestion(Question question);

        #endregion


        #region Select QuestionTag

        Task AddSelectQuestionTag(SelectQuestionTag selectQuestionTag);

        Task<Question?> GetQuestionById(long id);


        #endregion


        #region answer

        Task AddAnswer(Answer answer);

        Task<List<Answer>> GetAllQuestionAnswers(long questionId);

        Task<Answer?> GetAnswerById(long answeredId);

        Task UpdateAnswer(Answer answer);

        Task<bool> IsExistsUserScoreForAnswer(long answerId, long userId);

        Task<bool> IsExistsUserScoreForQuestion(long questionId, long userId);

        Task<bool> IsExistsQuestionInUserBookmarks(long questionId, long userId);

        Task<UserQuestionBookmark?> GetBookmarkByQuestionAndUserId(long questionId, long userId);

        Task AddAnswerUserScore(AnswerUserScore score);

        Task AddQuestionUserScore(QuestionUserScore score);

        void RemoveBookmark(UserQuestionBookmark bookmark);

        Task AddBookmark(UserQuestionBookmark bookmark);

        #endregion

        #region view

        Task<bool> IsExistsViewForQuestion(string userIp, long quetionsId);

        Task AddQuestionView(QuestionView questionView);

        #endregion

    }
}
