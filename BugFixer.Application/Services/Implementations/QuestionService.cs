using BugFixer.Application.Security;
using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.Tags;
using BugFixer.domain.InterFaces;
using BugFixer.domain.ViewModels.Common;
using BugFixer.domain.ViewModels.Question;
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

        public async Task<CreateQuestionResult> CheckTagValidation(List<string>? tags, long userId)
        {
            if (tags != null && tags.Any())
            {
                foreach (var tag in tags)
                {
                    var isExistsTag = await _questionRepository.IsExistsTagByName(tag.SanitizeText().Trim().ToLower());

                    if (isExistsTag) continue;

                    var isUserRequestedForTag =
                        await _questionRepository.CheckUserRequestedForTag(userId, tag.SanitizeText().Trim().ToLower());

                    if (isUserRequestedForTag)
                    {
                        return new CreateQuestionResult
                        {
                            Status = CreateQuestionResultEnum.NotValidTag,
                            Message = $"تگ {tag} برای اعتبارسنجی نیاز به {_scoreManagement.MinRequestCountForVerifyTag} درخواست دارد ."
                        };
                    }
                    var tagRequest = new RequestTag()
                    {
                        Title = tag.SanitizeText().Trim().ToLower(),
                        UserId = userId,
                    };

                    await _questionRepository.AddRequestTag(tagRequest);
                    await _questionRepository.SaveChanges();

                    var requestedCount =
                        await _questionRepository.RequestCountForTag(tag.SanitizeText().Trim().ToLower());

                    if (requestedCount < _scoreManagement.MinRequestCountForVerifyTag)
                    {
                        return new CreateQuestionResult
                        {
                            Status = CreateQuestionResultEnum.NotValidTag,
                            Message = $"تگ {tag} برای اعتبارسنجی نیاز به {_scoreManagement.MinRequestCountForVerifyTag} درخواست دارد ."
                        };
                    }

                    var newTag = new Tag
                    {
                        Title = tag.SanitizeText().Trim().ToLower()
                    };

                    await _questionRepository.AddTag(newTag);
                    await _questionRepository.SaveChanges();
                }

                return new CreateQuestionResult
                {
                    Status = CreateQuestionResultEnum.Success,
                    Message = "تگ های ورودی معتبر می باشد ."
                };
            }

            return new CreateQuestionResult
            {
                Status = CreateQuestionResultEnum.NotValidTag,
                Message = "تگ های ورودی نمی تواند خالی باشد ."
            };
        }

        public async Task<bool> CreateQuetion(CreateQuestionViewModel createQuestion)
        {
            var question = new Question()
            {
                Content = createQuestion.Description.SanitizeText(),
                Title = createQuestion.Title.SanitizeText(),
                UserId = createQuestion.UserId,
            };

            await _questionRepository.AddQuestion(question);
            await _questionRepository.SaveChanges();

            if (createQuestion.SelectedTags != null && createQuestion.SelectedTags.Any())
            {
                foreach (var quetionSelectedTag in createQuestion.SelectedTags)
                {
                    var tag = await _questionRepository.GetTagByName(quetionSelectedTag.SanitizeText().ToLower().Trim());

                    if (tag == null)
                    {
                        continue;
                    }

                    var selectedTag = new SelectQuestionTag()
                    {
                        QuestionId = question.Id,
                        TagId = tag.Id,
                    };

                    await _questionRepository.AddSelectQuestionTag(selectedTag);
                }
                await _questionRepository.SaveChanges();
            }

            return true;
        }

        #endregion
    }
}
