using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.Tags;
using System;
using System.Collections.Generic;
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

        #endregion

        #region quetion

        Task AddQuestion(Question question);

        #endregion

        #region Select QuestionTag

        Task AddSelectQuestionTag(SelectQuestionTag selectQuestionTag);

        #endregion
    }
}
