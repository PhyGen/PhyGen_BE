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

namespace teamseven.PhyGen.Services.Services.SemesterService
{
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SemesterService> _logger;

        public SemesterService(IUnitOfWork unitOfWork, ILogger<SemesterService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task CreateSemesterAsync(CreateSemesterRequest request)
        {
            var grade = await _unitOfWork.GradeRepository.GetByIdAsync(request.GradeId);
            if (grade == null)
            {
                _logger.LogWarning("Grade with ID {GradeId} not found.", request.GradeId);
                throw new NotFoundException($"Grade with ID {request.GradeId} not found.");
            }

            var semester = new Semester
            {
                Name = request.Name,
                GradeId = request.GradeId,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.SemesterRepository.CreateAsync(semester);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                _logger.LogInformation("Created semester with ID {Id}.", semester.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating semester.");
                throw new ApplicationException("An error occurred while creating the semester.", ex);
            }
        }

        public async Task DeleteSemesterAsync(int id)
        {
            var semester = await _unitOfWork.SemesterRepository.GetByIdAsync(id);
            if (semester == null)
            {
                _logger.LogWarning("Semester with ID {Id} not found for deletion.", id);
                throw new NotFoundException($"Semester with ID {id} not found.");
            }

            try
            {
                await _unitOfWork.SemesterRepository.RemoveAsync(semester);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                _logger.LogInformation("Deleted semester with ID {Id}.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting semester.");
                throw new ApplicationException("An error occurred while deleting the semester.", ex);
            }
        }
    }
}
