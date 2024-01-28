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
        Task<List<Tag>> GetAllTages();
    }
}
