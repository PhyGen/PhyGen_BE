using Microsoft.Extensions.Logging;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.UserSubscriptionService
{
    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserSubscriptionService> _logger;

        public UserSubscriptionService(IUnitOfWork unitOfWork, ILogger<UserSubscriptionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<UserSubscriptionDataResponse>> GetAllAsync()
        {
            var entities = await _unitOfWork.UserSubscriptionRepository.GetAllAsync();
            return entities.Select(us => new UserSubscriptionDataResponse
            {
                Id = us.Id,
                UserId = us.UserId,
                SubscriptionTypeId = us.SubscriptionTypeId,
                StartDate = us.StartDate,
                EndDate = us.EndDate,
                IsActive = us.IsActive,
                PaymentStatus = us.PaymentStatus,
                Amount = us.Amount,
                PaymentGatewayTransactionId = us.PaymentGatewayTransactionId,
                CreatedAt = us.CreatedAt,
                UpdatedAt = us.UpdatedAt
            });
        }

        public async Task<UserSubscriptionDataResponse> GetByIdAsync(int id)
        {
            var us = await _unitOfWork.UserSubscriptionRepository.GetByIdAsync(id);
            if (us == null)
                throw new NotFoundException($"UserSubscription with ID {id} not found.");

            return new UserSubscriptionDataResponse
            {
                Id = us.Id,
                UserId = us.UserId,
                SubscriptionTypeId = us.SubscriptionTypeId,
                StartDate = us.StartDate,
                EndDate = us.EndDate,
                IsActive = us.IsActive,
                PaymentStatus = us.PaymentStatus,
                Amount = us.Amount,
                PaymentGatewayTransactionId = us.PaymentGatewayTransactionId,
                CreatedAt = us.CreatedAt,
                UpdatedAt = us.UpdatedAt
            };
        }

        public async Task CreateAsync(CreateUserSubscriptionRequest request)
        {
            var entity = new UserSubscription
            {
                UserId = request.UserId,
                SubscriptionTypeId = request.SubscriptionTypeId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = request.IsActive,
                PaymentStatus = request.PaymentStatus,
                Amount = request.Amount,
                PaymentGatewayTransactionId = request.PaymentGatewayTransactionId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.UserSubscriptionRepository.CreateAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task UpdateAsync(UserSubscriptionDataRequest request)
        {
            var entity = await _unitOfWork.UserSubscriptionRepository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("UserSubscription not found.");

            entity.EndDate = request.EndDate;
            entity.IsActive = request.IsActive;
            entity.PaymentStatus = request.PaymentStatus;
            entity.Amount = request.Amount;
            entity.PaymentGatewayTransactionId = request.PaymentGatewayTransactionId;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.UserSubscriptionRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.UserSubscriptionRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("UserSubscription not found.");

            await _unitOfWork.UserSubscriptionRepository.RemoveAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }
    }
}
