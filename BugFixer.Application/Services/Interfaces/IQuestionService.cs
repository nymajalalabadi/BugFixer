using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.Tags;
using BugFixer.domain.ViewModels.Question;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion


        #region Answer

        Task<List<Answer>> GetAllQuestionAnswers(long questionId);

        #endregion

    }
}
