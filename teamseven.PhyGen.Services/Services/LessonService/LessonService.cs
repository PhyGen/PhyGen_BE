using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.LessonService
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LessonService> _logger;

        public LessonService(IUnitOfWork unitOfWork, ILogger<LessonService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<LessonDataResponse>> GetAllLessonAsync()
        {
            var lessons = await _unitOfWork.LessonRepository.GetAllAsync();
            return lessons.Select(l => new LessonDataResponse
            {
                Id = l.Id,
                Name = l.Name,
                ChapterId = l.ChapterId,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            });
        }
        public async Task<PagedResponse<LessonDataResponse>> GetLessonsAsync(int? pageNumber = null, int? pageSize = null)
        {
            try
            {
                List<Lesson> lessons;
                int totalItems;

                if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
                {
                    (lessons, totalItems) = await _unitOfWork.LessonRepository.GetPagedAsync(pageNumber.Value, pageSize.Value);
                }
                else
                {
                    lessons = await _unitOfWork.LessonRepository.GetAllAsync() ?? new List<Lesson>();
                    totalItems = lessons.Count;
                }

                var lessonResponses = lessons.Select(l => new LessonDataResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                    ChapterId = l.ChapterId,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                }).ToList();

                return new PagedResponse<LessonDataResponse>(lessonResponses, pageNumber ?? 1, pageSize ?? totalItems, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lessons: {Message}", ex.Message);
                throw new ApplicationException("An error occurred while retrieving lessons.", ex);
            }
        }
        public async Task<LessonDataResponse> GetLessonByIdAsync(int id)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);
            if (lesson == null)
            {
                _logger.LogWarning("Lesson with ID {Id} not found.", id);
                throw new NotFoundException($"Lesson with ID {id} not found.");
            }

            return new LessonDataResponse
            {
                Id = lesson.Id,
                Name = lesson.Name,
                ChapterId = lesson.ChapterId,
                CreatedAt = lesson.CreatedAt,
                UpdatedAt = lesson.UpdatedAt
            };
        }

        public async Task CreateLessonAsync(CreateLessonRequest request)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(request.ChapterId);
            if (chapter == null)
            {
                _logger.LogWarning("Chapter with ID {Id} not found.", request.ChapterId);
                throw new NotFoundException($"Chapter with ID {request.ChapterId} not found.");
            }

            var lesson = new Lesson
            {
                Name = request.Name,
                ChapterId = request.ChapterId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.LessonRepository.CreateAsync(lesson);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            _logger.LogInformation("Created lesson with ID {Id}.", lesson.Id);

            
        }

        public async Task UpdateLessonAsync(LessonDataRequest request)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(request.Id);
            if (lesson == null)
                throw new NotFoundException("Lesson not found");

            lesson.Name = request.Name;
            lesson.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.LessonRepository.UpdateAsync(lesson);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            _logger.LogInformation("Updated lesson with ID {Id}.", lesson.Id);
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);
            if (lesson == null)
            {
                _logger.LogWarning("Lesson with ID {Id} not found.", id);
                throw new NotFoundException($"Lesson with ID {id} not found.");
            }

            await _unitOfWork.LessonRepository.RemoveAsync(lesson);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            _logger.LogInformation("Deleted lesson with ID {Id}.", id);
        }
    }
}
