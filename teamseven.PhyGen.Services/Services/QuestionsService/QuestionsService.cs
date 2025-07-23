using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.QuestionsService
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<QuestionsService> _logger;

        public QuestionsService(IUnitOfWork unitOfWork, ILogger<QuestionsService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<QuestionDataResponse> AddQuestionAsync(QuestionDataRequest questionDataRequest)
        {
            if (questionDataRequest == null)
            {
                _logger.LogWarning("QuestionDataRequest is null.");
                throw new ArgumentNullException(nameof(questionDataRequest), "Question data request cannot be null.");
            }

            if (await _unitOfWork.UserRepository.GetByIdAsync(questionDataRequest.CreatedByUserId) == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", questionDataRequest.CreatedByUserId);
                throw new NotFoundException($"User with ID {questionDataRequest.CreatedByUserId} not found.");
            }

            var question = new Question
            {
                Content = questionDataRequest.Content,
                QuestionSource = questionDataRequest.QuestionSource,
                DifficultyLevel = questionDataRequest.DifficultyLevel,
                Image = questionDataRequest.Image,
                LessonId = questionDataRequest.LessonId,
                CreatedByUserId = questionDataRequest.CreatedByUserId,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.QuestionRepository.CreateAsync(question);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                _logger.LogInformation("Question with ID {QuestionId} created successfully.", question.Id);
                var response = new QuestionDataResponse
                {
                    Id = question.Id,
                    Content = question.Content,
                    QuestionSource = question.QuestionSource,
                    DifficultyLevel = question.DifficultyLevel,
                    LessonId = question.LessonId,
                    CreatedByUserId = question.CreatedByUserId,
                    CreatedAt = question.CreatedAt,
                    UpdatedAt = question.UpdatedAt
                };

                return response;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question: {Message}", ex.Message);
                throw new ApplicationException("An error occurred while creating the question.", ex);
            }
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var target = await _unitOfWork.QuestionRepository.GetByIdAsync(id);
            if (target == null)
            {
                _logger.LogWarning("Question with ID {QuestionId} not found.", id);
                throw new NotFoundException($"Question with ID {id} not found.");
            }

            await _unitOfWork.QuestionRepository.RemoveAsync(target);
            await _unitOfWork.SaveChangesWithTransactionAsync();
            _logger.LogInformation("Question with ID {QuestionId} deleted successfully.", id);
        }

        public async Task<QuestionDataResponse> GetQuestionById(int id)
        {
            var question = await _unitOfWork.QuestionRepository.GetByIdAsync(id);
            if (question == null)
                throw new NotFoundException($"Question with ID {id} not found.");

            return new QuestionDataResponse
            {
                Id = question.Id,
                Content = question.Content,
                QuestionSource = question.QuestionSource,
                DifficultyLevel = question.DifficultyLevel,
                Image = question.Image,
                LessonId = question.LessonId,
                CreatedByUserId = question.CreatedByUserId,
                CreatedAt = question.CreatedAt,
                UpdatedAt = question.UpdatedAt,
                LessonName = question.Lesson?.Name,
                CreatedByUserName = question.CreatedByUser?.Email
            };
        }

        public Task ModifyQuestionAsync(QuestionDataRequest questionDataRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<QuestionDataResponse>> GetQuestionsAsync(
            int? pageNumber = null,
            int? pageSize = null,
            string? search = null,
            string? sort = null,
            int? lessonId = null,
            string? difficultyLevel = null,
            int? chapterId = null,
            int isSort = 0) // Default isSort = 0 (No)
        {
            try
            {
                List<Question> questions;
                int totalItems;

                if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
                {
                    (questions, totalItems) = await _unitOfWork.QuestionRepository.GetPagedAsync(
                        pageNumber.Value,
                        pageSize.Value,
                        search,
                        sort,
                        lessonId,
                        difficultyLevel,
                        chapterId,
                        isSort);
                }
                else
                {
                    questions = await _unitOfWork.QuestionRepository.GetAllAsync() ?? new List<Question>();
                    if (!string.IsNullOrEmpty(search))
                    {
                        var searchNormalized = search.RemoveDiacritics().ToLower();
                        questions = questions.Where(q => q.Content.RemoveDiacritics().ToLower().Contains(searchNormalized) ||
                                                       q.QuestionSource.RemoveDiacritics().ToLower().Contains(searchNormalized)).ToList();
                    }
                    if (lessonId.HasValue)
                    {
                        questions = questions.Where(q => q.LessonId == lessonId.Value).ToList();
                    }
                    if (!string.IsNullOrEmpty(difficultyLevel))
                    {
                        questions = questions.Where(q => q.DifficultyLevel == difficultyLevel).ToList();
                    }
                    if (chapterId.HasValue)
                    {
                        var lessonIds = await _unitOfWork.LessonRepository.GetAllAsync();
                        lessonIds = lessonIds.Where(l => l.ChapterId == chapterId.Value).ToList();
                        questions = questions.Where(q => lessonIds.Any(l => l.Id == q.LessonId)).ToList();
                    }
                    if (isSort == 1)
                    {
                        questions = sort?.ToLower() switch
                        {
                            "content:asc" => questions.OrderBy(q => q.Content).ToList(),
                            "content:desc" => questions.OrderByDescending(q => q.Content).ToList(),
                            "difficultylevel:asc" => questions.OrderBy(q => q.DifficultyLevel).ToList(),
                            "difficultylevel:desc" => questions.OrderByDescending(q => q.DifficultyLevel).ToList(),
                            "createdat:asc" => questions.OrderBy(q => q.CreatedAt).ToList(),
                            "createdat:desc" => questions.OrderByDescending(q => q.CreatedAt).ToList(),
                            "updatedat:asc" => questions.OrderBy(q => q.UpdatedAt).ToList(),
                            "updatedat:desc" => questions.OrderByDescending(q => q.UpdatedAt).ToList(),
                            _ => questions.OrderByDescending(q => q.CreatedAt).ToList()
                        };
                    }
                    else
                    {
                        questions = questions.OrderBy(q => q.Id).ToList();
                    }
                    totalItems = questions.Count;
                }

                var questionResponses = questions.Select(q => new QuestionDataResponse
                {
                    Id = q.Id,
                    Content = q.Content,
                    QuestionSource = q.QuestionSource,
                    DifficultyLevel = q.DifficultyLevel,
                    LessonId = q.LessonId,
                    CreatedByUserId = q.CreatedByUserId,
                    CreatedAt = q.CreatedAt,
                    UpdatedAt = q.UpdatedAt
                }).ToList();

                return new PagedResponse<QuestionDataResponse>(
                    questionResponses,
                    pageNumber ?? 1,
                    pageSize ?? totalItems,
                    totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving questions: {Message}", ex.Message);
                throw new ApplicationException("Error retrieving questions.", ex);
            }
        }
    }
}