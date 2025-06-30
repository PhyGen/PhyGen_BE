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

namespace teamseven.PhyGen.Services.Services.UserSocialProviderService
{
    public class UserSocialProviderService : IUserSocialProviderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserSocialProviderService> _logger;

        public UserSocialProviderService(IUnitOfWork unitOfWork, ILogger<UserSocialProviderService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(UserSocialProviderRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("UserSocialProviderRequest is null.");
                throw new ArgumentNullException(nameof(request));
            }

            if (await _unitOfWork.UserRepository.GetByIdAsync(request.UserId) == null)
            {
                throw new NotFoundException($"User with ID {request.UserId} not found.");
            }

            var entity = new UserSocialProvider
            {
                UserId = request.UserId,
                ProviderName = request.ProviderName,
                ProviderId = request.ProviderId,
                Email = request.Email,
                ProfileUrl = request.ProfileUrl,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.UserSocialProviderRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                _logger.LogInformation("UserSocialProvider added with ID {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding UserSocialProvider");
                throw new ApplicationException("An error occurred while adding the user social provider.", ex);
            }
        }

        public async Task<UserSocialProviderResponse> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.UserSocialProviderRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"UserSocialProvider with ID {id} not found.");
            }

            return new UserSocialProviderResponse
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ProviderName = entity.ProviderName,
                ProviderId = entity.ProviderId,
                Email = entity.Email,
                ProfileUrl = entity.ProfileUrl,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public async Task<List<UserSocialProviderResponse>> GetByUserIdAsync(int userId)
        {
            var list = await _unitOfWork.UserSocialProviderRepository.GetByUserIdAsync(userId);
            return list?.Select(entity => new UserSocialProviderResponse
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ProviderName = entity.ProviderName,
                ProviderId = entity.ProviderId,
                Email = entity.Email,
                ProfileUrl = entity.ProfileUrl,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            }).ToList() ?? new List<UserSocialProviderResponse>();
        }

        public async Task UpdateAsync(UserSocialProviderRequest request)
        {
            var entity = await _unitOfWork.UserSocialProviderRepository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException($"UserSocialProvider with ID {request.Id} not found.");
            }

            entity.ProviderName = request.ProviderName;
            entity.ProviderId = request.ProviderId;
            entity.Email = request.Email;
            entity.ProfileUrl = request.ProfileUrl;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.UserSocialProviderRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.UserSocialProviderRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"UserSocialProvider with ID {id} not found.");
            }

            await _unitOfWork.UserSocialProviderRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }
    }
}
