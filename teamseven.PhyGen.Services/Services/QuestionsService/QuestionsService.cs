﻿using Microsoft.Extensions.Logging;
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

        public async Task<QuestionDataResponse> ModifyQuestionAsync(UpdateQuestionRequest questionDataRequest)
        {
            if (questionDataRequest == null)
            {
                _logger.LogWarning("QuestionDataRequest is null.");
                throw new ArgumentNullException(nameof(questionDataRequest));
            }

            var existingQuestion = await _unitOfWork.QuestionRepository.GetByIdAsync(questionDataRequest.Id);
            if (existingQuestion == null)
            {
                _logger.LogWarning("Question with ID {QuestionId} not found.", questionDataRequest.Id);
                throw new NotFoundException($"Question with ID {questionDataRequest.Id} not found.");
            }

            existingQuestion.Content = questionDataRequest.Content;
            //existingQuestion.QuestionSource = questionDataRequest.QuestionSource;
            existingQuestion.DifficultyLevel = questionDataRequest.DifficultyLevel;
            //existingQuestion.Image = questionDataRequest.Image;
            //existingQuestion.LessonId = questionDataRequest.LessonId;
            existingQuestion.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _unitOfWork.QuestionRepository.UpdateAsync(existingQuestion);
                await _unitOfWork.SaveChangesWithTransactionAsync();

                _logger.LogInformation("Question with ID {QuestionId} updated successfully.", existingQuestion.Id);

                // ✅ Trả về dữ liệu sau khi cập nhật
                return new QuestionDataResponse
                {
                    Id = existingQuestion.Id,
                    Content = existingQuestion.Content,
                    QuestionSource = existingQuestion.QuestionSource,
                    DifficultyLevel = existingQuestion.DifficultyLevel,
                    Image = existingQuestion.Image,
                    LessonId = existingQuestion.LessonId,
                    CreatedAt = existingQuestion.CreatedAt,
                    UpdatedAt = existingQuestion.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question with ID {QuestionId}: {Message}", existingQuestion.Id, ex.Message);
                throw new ApplicationException($"Error updating question with ID {existingQuestion.Id}", ex);
            }
        }




        public async Task<PagedResponse<QuestionDataResponse>> GetQuestionsAsync(
    int? pageNumber = null,
    int? pageSize = null,
    string? search = null,
    string? sort = null,
    int? lessonId = null,
    string? difficultyLevel = null,
    int? chapterId = null,
    int isSort = 0,
    int? createdByUserId = null) // Thêm lọc theo user
        {
            try
            {
                List<Question> questions;
                int totalItems;

                if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
                {
                    // Nếu bạn có sửa GetPagedAsync để truyền thêm createdByUserId thì truyền vào đây
                    (questions, totalItems) = await _unitOfWork.QuestionRepository.GetPagedAsync(
                        pageNumber.Value,
                        pageSize.Value,
                        search,
                        sort,
                        lessonId,
                        difficultyLevel,
                        chapterId,
                        isSort,
                        createdByUserId ?? 0); // cần thêm nếu repo có xử lý
                }
                else
                {
                    questions = await _unitOfWork.QuestionRepository.GetAllAsync() ?? new List<Question>();

                    // Lọc theo user tạo
                    if (createdByUserId.HasValue)
                    {
                        questions = questions.Where(q => q.CreatedByUserId == createdByUserId.Value).ToList();
                    }

                    // Lọc theo search
                    if (!string.IsNullOrEmpty(search))
                    {
                        var searchNormalized = search.RemoveDiacritics().ToLower();
                        questions = questions.Where(q =>
                            q.Content.RemoveDiacritics().ToLower().Contains(searchNormalized) ||
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
                var allLessons = await _unitOfWork.LessonRepository.GetAllAsync();
                var allUsers = await _unitOfWork.UserRepository.GetAllAsync();
                var allChapters = await _unitOfWork.ChapterRepository.GetAllAsync();
                var questionResponses = questions.Select(q => new QuestionDataResponse
                {
                    Id = q.Id,
                    Content = q.Content,
                    QuestionSource = q.QuestionSource,
                    DifficultyLevel = q.DifficultyLevel,
                    LessonId = q.LessonId,
                    ChapterId = q.Lesson?.ChapterId,
                    CreatedByUserId = q.CreatedByUserId,
                    CreatedAt = q.CreatedAt,
                    UpdatedAt = q.UpdatedAt,
                    LessonName = allLessons.FirstOrDefault(l => l.Id == q.LessonId)?.Name ?? string.Empty,
                    ChapterName = allChapters.FirstOrDefault(c => c.Id == q.Lesson?.ChapterId)?.Name ?? string.Empty,
                    CreatedByUserName = allUsers.FirstOrDefault(u => u.Id == q.CreatedByUserId)?.Email ?? string.Empty
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