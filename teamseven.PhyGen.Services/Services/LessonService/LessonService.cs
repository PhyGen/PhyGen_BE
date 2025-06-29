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
