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

namespace teamseven.PhyGen.Services.Services.ChapterService
{
    public class ChapterService : IChapterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChapterService> _logger;

        public ChapterService(IUnitOfWork unitOfWork, ILogger<ChapterService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task CreateChapterAsync(CreateChapterRequest request)
        {
            var semester = await _unitOfWork.SemesterRepository.GetByIdAsync(request.SemesterId);
            if (semester == null)
            {
                _logger.LogWarning("Semester with ID {SemesterId} not found.", request.SemesterId);
                throw new NotFoundException($"Semester with ID {request.SemesterId} not found.");
            }

            var chapter = new Chapter
            {
                Name = request.Name,
                SemesterId = request.SemesterId,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.ChapterRepository.CreateAsync(chapter);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                _logger.LogInformation("Created chapter with ID {Id}.", chapter.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating chapter.");
                throw new ApplicationException("An error occurred while creating the chapter.", ex);
            }
        }

        public async Task DeleteChapterAsync(int id)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(id);
            if (chapter == null)
            {
                _logger.LogWarning("Chapter with ID {Id} not found for deletion.", id);
                throw new NotFoundException($"Chapter with ID {id} not found.");
            }

            try
            {
                await _unitOfWork.ChapterRepository.RemoveAsync(chapter);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                _logger.LogInformation("Deleted chapter with ID {Id}.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting chapter.");
                throw new ApplicationException("An error occurred while deleting the chapter.", ex);
            }
        }
    }
}
