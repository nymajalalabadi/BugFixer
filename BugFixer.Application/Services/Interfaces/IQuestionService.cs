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

    }
}
