using BugFixer.Application.Extensions;
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
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        #region onstractor

        private readonly IQuestionRepository _questionRepository;

        private readonly IUserService _userService;

        private ScoreManagementViewModel _scoreManagement;

        public QuestionService(IQuestionRepository questionRepository, IUserService userService, IOptions<ScoreManagementViewModel> scoreManagement)
        {
            _questionRepository = questionRepository;
            _userService = userService;
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

                    tag.UseCount += 1;

                    await _questionRepository.UpdateTag(tag);

                    var selectedTag = new SelectQuestionTag()
                    {
                        QuestionId = question.Id,
                        TagId = tag.Id,
                    };

                    await _questionRepository.AddSelectQuestionTag(selectedTag);
                }
                await _questionRepository.SaveChanges();
            }

            await _userService.UpdateUserScoreAndMedal(createQuestion.UserId, _scoreManagement.AddNewQuestionScore);

            return true;
        }

        #endregion

        #region quetion

        public async Task<FilterQuestionViewModel> FilterQuestion(FilterQuestionViewModel filterQuestion)
        {
            var query = await _questionRepository.GetAllQuestions();

            #region filter by tag

            if (!string.IsNullOrEmpty(filterQuestion.TagTitle))
            {
                query = query.Include(q => q.SelectQuestionTags).ThenInclude(s => s.Tag)
                    .Where(s => s.SelectQuestionTags.Any(s => s.Tag.Title.Equals(filterQuestion.TagTitle)));
            }

            #endregion


            #region filter

            if (!string.IsNullOrEmpty(filterQuestion.Title))
            {
                query = query.Where(q => q.Title.Contains(filterQuestion.Title.SanitizeText().Trim()));
            }

            #endregion

            #region sort

            switch (filterQuestion.Sort)
            {
                case FilterQuestionSortEnum.NewToOld:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterQuestionSortEnum.OldToNew:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterQuestionSortEnum.ScoreHighToLow:
                    query = query.OrderByDescending(s => s.Score);
                    break;
                case FilterQuestionSortEnum.ScoreLowToHigh:
                    query = query.OrderBy(s => s.Score);
                    break;
            }

            #endregion

            var result = query
                .Include(s => s.Answers)
                .Include(s => s.SelectQuestionTags)
                .ThenInclude(a => a.Tag)
                .Include(s => s.User)
                .Select(s => new QuestionListViewModel()
                {
                    AnswersCount = s.Answers.Count(a => !a.IsDelete),
                    HasAnyAnswer = s.Answers.Any(a => !a.IsDelete),
                    HasAnyTrueAnswer = s.Answers.Any(a => !a.IsDelete && a.IsTrue),
                    QuestionId = s.Id,
                    Score = s.Score,
                    Title = s.Title,
                    ViewCount = s.ViewCount,
                    UserDisplayName = s.User.GetUserDisplayName(),
                    Tags = s.SelectQuestionTags.Where(a => !a.Tag.IsDelete).Select(a => a.Tag.Title).ToList(),
                    AnswerByDisplayName = s.Answers.Any(a => !a.IsDelete) ? 
                    s.Answers.OrderByDescending(a => a.CreateDate).First().User.GetUserDisplayName() : null,
                    CreateDate = s.CreateDate.TimeAgo(),
                    AnswerByCreateDate = s.Answers.Any(a => !a.IsDelete) ? 
                    s.Answers.OrderByDescending(a => a.CreateDate).First().CreateDate.TimeAgo() : null
                }).AsQueryable();


            #region set paging

            await filterQuestion.SetPaging(result);

            return filterQuestion;

            #endregion
        }

        public async Task<FilterTagViewModel> FilterTags(FilterTagViewModel filterTags)
        {
            var query = await _questionRepository.GetAllTagsQueryable();

            #region filter

            if (!string.IsNullOrEmpty(filterTags.Title))
            {
                query = query.Where(t => t.Title.Contains(filterTags.Title.SanitizeText().Trim()));   
            }

            #endregion

            #region sort

            switch (filterTags.Sort)
            {
                case FilterTagEnum.NewToOld:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterTagEnum.OldToNew:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterTagEnum.UseCountHighToLow:
                    query = query.OrderByDescending(s => s.UseCount);
                    break;
                case FilterTagEnum.UseCountLowToHigh:
                    query = query.OrderBy(s => s.UseCount);
                    break;
            }

            #endregion

            #region set paging

            await filterTags.SetPaging(query);

            return filterTags;

            #endregion
        }


        public async Task<Question?> GetQuestionById(long id)
        {
            return await _questionRepository.GetQuestionById(id);
        }

        public async Task<List<string>> GetTagListForQuestionId(long quetionsId)
        {
            return await _questionRepository.GetTagListForQuestionId(quetionsId);
        }

        public async Task<bool> AnswerQuestion(AnswerQuestionViewModel answerQuestion)
        {
            var question = await GetQuestionById(answerQuestion.QuestionId);

            if (question == null)
            {
                return false;
            }

            var answer = new Answer()
            {
                Content = answerQuestion.Answer.SanitizeText(),
                QuestionId = answerQuestion.QuestionId,
                UserId = answerQuestion.UserId
            };

            await _questionRepository.AddAnswer(answer);
            await _questionRepository.SaveChanges();

            await _userService.UpdateUserScoreAndMedal(answerQuestion.UserId, _scoreManagement.AddNewAnswerScore);

            return true;
        }

        public async Task AddViewForQuestion(string userIp, Question question)
        {
            if (await _questionRepository.IsExistsViewForQuestion(userIp, question.Id))
            {
                return;
            }

            var view = new QuestionView()
            {
                QuestionId = question.Id,
                UserIP = userIp
            };

            await _questionRepository.AddQuestionView(view);

            question.ViewCount += 1;

            await _questionRepository.UpdateQuestion(question);

            await _questionRepository.SaveChanges();

        }

        #endregion


        #region Answer

        public async Task<List<Answer>> GetAllQuestionAnswers(long questionId)
        {
            return await _questionRepository.GetAllQuestionAnswers(questionId);
        }

        public async Task<bool> HasUserAccessToSelectTrueAnswer(long userId, long answerId)
        {
            var answer = await _questionRepository.GetAnswerById(answerId);

            if (answer == null) return false;

            var user = await _userService.GetUserById(userId);

            if (user == null) return false;

            if (user.IsAdmin) return true;

            if (answer.Question.UserId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task SelectTrueAnswer(long userId, long answerId)
        {
            var answer = await _questionRepository.GetAnswerById(answerId);

            if (answer == null) return;

            answer.IsTrue = !answer.IsTrue;

            await _questionRepository.UpdateAnswer(answer);
            await _questionRepository.SaveChanges();
        }

        #endregion
    }
}
