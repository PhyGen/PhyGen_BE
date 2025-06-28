using Microsoft.Extensions.Logging;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Services.Services.GradeService
{
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GradeService> _logger;

        public GradeService(IUnitOfWork unitOfWork, ILogger<GradeService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task CreateGradeAsync(CreateGradeRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("CreateGradeRequest is null.");
                throw new ArgumentNullException(nameof(request), "Grade creation request cannot be null.");
            }

            var grade = new Grade
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.GradeRepository.CreateAsync(grade);
                await _unitOfWork.SaveChangesWithTransactionAsync();

                _logger.LogInformation("Created grade with ID {GradeId}.", grade.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating grade: {Message}", ex.Message);
                throw new ApplicationException("An error occurred while creating the grade.", ex);
            }
        }



        public async Task DeleteGradeAsync(int id)
        {
            var grade = await _unitOfWork.GradeRepository.GetByIdAsync(id);
            if (grade == null)
                throw new NotFoundException($"Grade with ID {id} not found.");

            await _unitOfWork.GradeRepository.RemoveAsync(grade);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            _logger.LogInformation("Deleted grade with ID {Id}.", grade.Id);
        }
    }
}