using BugFixer.domain.Entities.Tags;
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

        #endregion

    }
}
