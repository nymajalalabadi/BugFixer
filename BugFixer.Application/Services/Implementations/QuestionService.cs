using BugFixer.Application.Extensions;
using BugFixer.Application.Security;
using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.Statics;
using BugFixer.domain.Entities.Account;
using BugFixer.domain.Entities.Questions;
using BugFixer.domain.Entities.Tags;
using BugFixer.domain.Enums;
using BugFixer.domain.InterFaces;
using BugFixer.domain.ViewModels.Admin.Tag;
using BugFixer.domain.ViewModels.Common;
using BugFixer.domain.ViewModels.Question;
using BugFixer.domain.ViewModels.UserPanel.Question;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
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

        #endregion

        #region quetion
        public async Task<IQueryable<Question>> GetAllQuestions()
        {
            return await _questionRepository.GetAllQuestions();
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

        public async Task<bool> EditQuetion(EditQuestionViewModel editQuestion)
        {
            var question = await _questionRepository.GetQuestionById(editQuestion.Id);

            if (question == null)
            {
                return false;
            }

            var user = await _userService.GetUserById(editQuestion.UserId);

            if (user == null)
            {
                return false;
            }

            if (question.UserId != editQuestion.UserId && !user.IsAdmin)
            {
                return false;
            }

            #region Delete Avatar Editor

            FileExtensions.ManageEditorImages(question.Content, editQuestion.Description, PathTools.EditorImageServerPath);

            #endregion

            question.Title = editQuestion.Title;
            question.Content = editQuestion.Description;

            #region Delete Current Tags

            var currentTags = question.SelectQuestionTags.ToList();

            foreach (var tag in currentTags)
            {
                tag.Tag.UseCount -= 1;
                await _questionRepository.UpdateTag(tag.Tag);

                await _questionRepository.RemoveSelectQuestionTag(tag);
            }

            await _questionRepository.SaveChanges();

            #endregion

            #region Add New Tag

            if (editQuestion.SelectedTags != null && editQuestion.SelectedTags.Any())
            {
                foreach (var quetionSelectedTag in editQuestion.SelectedTags)
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

            #endregion

            await _questionRepository.UpdateQuestion(question);
            await _questionRepository.SaveChanges();

            return true;

        }

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

        public async Task<FilterQuestionBookmarksViewModel> FilterQuestionBookmarks(FilterQuestionBookmarksViewModel filterQuestion)
        {
            var query = _questionRepository.GetAllBookmarks();

            query = query.Where(s => s.UserId == filterQuestion.UserId);

            #region set paging

            await filterQuestion.SetPaging(query.Select(s => s.Question).AsQueryable());

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

        public async Task<bool> AddQuestionToBookmark(long questionId, long userId)
        {
            var question = await GetQuestionById(questionId);

            if (question == null)
            {
                return false;
            }

            if (await _questionRepository.IsExistsQuestionInUserBookmarks(questionId, userId))
            {
                var bookmark = await _questionRepository.GetBookmarkByQuestionAndUserId(questionId, userId);

                if (bookmark == null)
                {
                    return false;
                }
                _questionRepository.RemoveBookmark(bookmark);
            }
            else
            {
                var newBookmark = new UserQuestionBookmark
                {
                    QuestionId = questionId,
                    UserId = userId
                };

                await _questionRepository.AddBookmark(newBookmark);
            }

            await _questionRepository.SaveChanges();

            return true;
        }

        public async Task<bool> IsExistsQuestionInUserBookmarks(long questionId, long userId)
        {
            return await _questionRepository.IsExistsQuestionInUserBookmarks(questionId, userId);
        }

        public async Task<EditQuestionViewModel?> FillEditQuestionViewModel(long questionId, long userId)
        {
            var question = await GetQuestionById(questionId);

            if (question == null) return null;

            var user = await _userService.GetUserById(userId);

            if (user == null) return null;

            if (question.UserId != user.Id && !user.IsAdmin)
            {
                return null;
            }

            var tags = await GetTagListForQuestionId(questionId);

            var result = new EditQuestionViewModel
            {
                Id = question.Id,
                Description = question.Content,
                Title = question.Title,
                SelectedTagsJson = JsonConvert.SerializeObject(tags)
            };

            return result;
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

        public async Task<CreateScoreForAnswerResult> CreateScoreForAnswer(long asnwerId, AnswerScoreType type, long userId)
        {
            var answer = await _questionRepository.GetAnswerById(asnwerId);

            if (answer == null)
            {
                return CreateScoreForAnswerResult.Error;
            }

            var user = await _userService.GetUserById(userId);

            if (user == null)
            {
                return CreateScoreForAnswerResult.Error;
            }

            if (type == AnswerScoreType.Minus && user.Score < _scoreManagement.MinScoreForDownScoreAnswer)
            {
                return CreateScoreForAnswerResult.NotEnoughScoreForDown;
            }

            if (type == AnswerScoreType.Plus && user.Score < _scoreManagement.MinScoreForUpScoreAnswer)
            {
                return CreateScoreForAnswerResult.NotEnoughScoreForUp;
            }

            if (await _questionRepository.IsExistsUserScoreForAnswer(asnwerId, userId))
            {
                return CreateScoreForAnswerResult.UserCreateScoreBefore;
            }

            var score = new AnswerUserScore()
            {
                AnswerId = asnwerId,
                UserId = userId,
                Type = type,
            };

            await _questionRepository.AddAnswerUserScore(score);

            if (type == AnswerScoreType.Minus)
            {
                answer.Score -= 1;
            }
            else
            {
                answer.Score += 1;
            }

            await _questionRepository.UpdateAnswer(answer);

            await _questionRepository.SaveChanges();

            return CreateScoreForAnswerResult.Success;
        }

        public async Task<CreateScoreForQuestionResult> CreateScoreForQuestion(long questionId, QuestionScoreType type, long userId)
        {
            var question = await GetQuestionById(questionId);

            if (question == null)
            {
                return CreateScoreForQuestionResult.Error;
            }

            var user = await _userService.GetUserById(userId);

            if (user == null)
            {
                return CreateScoreForQuestionResult.Error;
            }

            if (type == QuestionScoreType.Minus && user.Score < _scoreManagement.MinScoreForDownScoreQuestion)
            {
                return CreateScoreForQuestionResult.NotEnoughScoreForDown;
            }

            if (type == QuestionScoreType.Plus && user.Score < _scoreManagement.MinScoreForUpScoreQuestion)
            {
                return CreateScoreForQuestionResult.NotEnoughScoreForUp;
            }

            if (await _questionRepository.IsExistsUserScoreForQuestion(questionId, userId))
            {
                return CreateScoreForQuestionResult.UserCreateScoreBefore;
            }

            var score = new QuestionUserScore
            {
                QuestionId = questionId,
                UserId = userId,
                Type = type
            };

            await _questionRepository.AddQuestionUserScore(score);

            if (type == QuestionScoreType.Minus)
            {
                question.Score -= 1;
            }
            else
            {
                question.Score += 1;
            }

            await _questionRepository.UpdateQuestion(question);

            await _questionRepository.SaveChanges();

            return CreateScoreForQuestionResult.Success;
        }

        public async Task<EditAnswerViewModel?> FillEditAnswerViewModel(long answerId, long userId)
        {
            var answer = await _questionRepository.GetAnswerById(answerId);

            if (answer == null)
            {
                return null;
            }

            var user = await _userService.GetUserById(userId);

            if (user == null) return null;

            if (answer.UserId != user.Id && !user.IsAdmin)
            {
                return null;
            }

            var result = new EditAnswerViewModel()
            {
                AnswerId = answer.Id,
                QuestionId = answer.QuestionId,
                Answer = answer.Content
            };

            return result;
        }

        public async Task<bool> EditAnswer(EditAnswerViewModel editAnswerViewModel)
        {
            var answer = await _questionRepository.GetAnswerById(editAnswerViewModel.AnswerId);

            if (answer == null)
            {
                return false;
            }

            if (answer.QuestionId != editAnswerViewModel.QuestionId)
            {
                return false;
            }

            var user = await _userService.GetUserById(editAnswerViewModel.UserId);

            if (user == null) return false;

            if (answer.UserId != user.Id && !user.IsAdmin)
            {
                return false;
            }

            #region Delete Avatar Editor

            FileExtensions.ManageEditorImages(answer.Content, editAnswerViewModel.Answer, PathTools.EditorImageServerPath);

            #endregion

            answer.Content = editAnswerViewModel.Answer;

            await _questionRepository.UpdateAnswer(answer);
            await _questionRepository.SaveChanges();

            return true;

        }

        #endregion


        #region Admin

        public async Task<List<TagViewModelJson>> GetTagViewModelJson()
        {
            var tags = await _questionRepository.GetAllTagsQueryable();

            return tags.OrderByDescending(t => t.UseCount)
                .Take(8)
                .Select(t => new TagViewModelJson()
                {
                    Title = t.Title,
                    UseCount = t.UseCount,
                })
                .ToList();
        }

        public async Task<FilterTagAdminViewModel> FilterTagAdmin(FilterTagAdminViewModel filter)
        {
            var query = await _questionRepository.GetAllTagsQueryable();

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => s.Title.Contains(filter.Title));
            }

            switch (filter.Status)
            {
                case FilterTagAdminStatus.All:
                    break;
                case FilterTagAdminStatus.HasDescription:
                    query = query.Where(s => !string.IsNullOrEmpty(s.Description));
                    break;
                case FilterTagAdminStatus.NoDescription:
                    query = query.Where(s => string.IsNullOrEmpty(s.Description));
                    break;
            }

            #region paging

            await filter.SetPaging(query);

            #endregion

            return filter;

        }

        #endregion
    }
}
