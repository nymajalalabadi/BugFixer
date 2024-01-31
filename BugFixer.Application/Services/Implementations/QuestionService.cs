using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.Entities.Tags;
using BugFixer.domain.InterFaces;
using BugFixer.domain.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        #region onstractor

        private readonly IQuestionRepository _questionRepository;

        private ScoreManagementViewModel _scoreManagement;

        public QuestionService(IQuestionRepository questionRepository, IOptions<ScoreManagementViewModel> scoreManagement) 
        {
            _questionRepository = questionRepository;
            _scoreManagement = scoreManagement.Value;
        }

        #endregion

        #region tag

        public async Task<List<Tag>> GetAllTages()
        {
            return await _questionRepository.GetAllTages();
        }

        #endregion
    }
}
