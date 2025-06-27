using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Extensions;

namespace teamseven.PhyGen.Services.Services.QuestionsService
{
    public class QuestionsService : IQuestionsService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public QuestionsService(IUnitOfWork unitOfWork, ILogger<QuestionsService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task AddQuestionAsync(QuestionDataRequest questionDataRequest)
        {
            // check request exist
            if (questionDataRequest == null)
            {
                _logger.LogWarning("QuestionDataRequest is null.");
                throw new ArgumentNullException(nameof(questionDataRequest), "Question data request cannot be null.");
            }

            // check exist Lession
            if (await _unitOfWork.LessonRepository.GetByIdAsync(questionDataRequest.LessonId) == null)
            {
                _logger.LogWarning("Lesson with ID {LessonId} not found.", questionDataRequest.LessonId);
                throw new NotFoundException($"Lesson with ID {questionDataRequest.LessonId} not found.");
            }

            // check user exist
            if (await _unitOfWork.UserRepository.GetByIdAsync(questionDataRequest.CreatedByUserId) == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", questionDataRequest.CreatedByUserId);
                throw new NotFoundException($"User with ID {questionDataRequest.CreatedByUserId} not found.");
            }

            // mapping to entity
            var question = new Question
            {
                Content = questionDataRequest.Content,
                QuestionSource = questionDataRequest.QuestionSource,
                DifficultyLevel = questionDataRequest.DifficultyLevel,
                LessonId = questionDataRequest.LessonId,
                CreatedByUserId = questionDataRequest.CreatedByUserId,
                CreatedAt = DateTime.UtcNow 
            };
            try
            {
              
                await _unitOfWork.QuestionRepository.CreateAsync(question);

                // save with transactions
                await _unitOfWork.SaveChangesWithTransactionAsync();

                _logger.LogInformation("Question with ID {QuestionId} created successfully.", question.Id);

                //// mapping to Respone
                //return new QuestionDataResponse
                //{
                //    Id = question.Id,
                //    Content = question.Content,
                //    QuestionSource = question.QuestionSource,
                //    DifficultyLevel = question.DifficultyLevel,
                //    LessonId = question.LessonId,
                //    CreatedByUserId = question.CreatedByUserId,
                //    CreatedAt = question.CreatedAt,
                //    UpdatedAt = question.UpdatedAt,
                //    //CreatedByUserName = question.Crea
                //    //LessonName = lesson.Name 
                //};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question: {Message}", ex.Message);
                throw new ApplicationException("An error occurred while creating the question.", ex);
            }
        }
   

        public async Task DeleteQuestionAsync(int id)
        {
           //check id exsit
           var target = await _unitOfWork.QuestionRepository.GetByIdAsync(id);
            if (target == null) { throw new NotFoundException($"Not found id {id} in database"); }
            else
            {
                await _unitOfWork.QuestionRepository.RemoveAsync(target);

                await _unitOfWork.SaveChangesWithTransactionAsync();
            }
        }

        public Task<QuestionDataResponse> GetQuestionById(int id)
        {
            throw new NotImplementedException();
        }

        public Task ModifyQuestionAsync(QuestionDataRequest questionDataRequest)
        {
            throw new NotImplementedException();
        }
    }
}
