﻿using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Repository.Repository;
using teamseven.PhyGen.Repository.Repository.Interfaces;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Interfaces;
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

        public async Task AddSubscriptionAsync(UserSubscriptionRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("CreateSubscriptionRequest is null.");
                throw new ArgumentNullException(nameof(request), "Subsription creation request cannot be null.");
            }
            var userSubscription = new UserSubscription
            {
                UserId = request.UserId,
                SubscriptionTypeId = request.SubscriptionTypeId,
                StartDate = DateTime.UtcNow,
                EndDate = request.EndDate ?? DateTime.UtcNow.AddMonths(1),
                IsActive = false,
                PaymentStatus = "Pending",
                Amount = request.Amount,
                PaymentGatewayTransactionId = request.PaymentGatewayTransactionId,
                CreatedAt = DateTime.UtcNow
            };
            try
            {
                await _unitOfWork.UserSubscriptionRepository.CreateAsync(userSubscription);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                _logger.LogInformation("Created sub with ID {SubId}.", userSubscription.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sub: {Message}", ex.Message);
                throw new ApplicationException("An error occurred while creating the sub.", ex);
            }
        }

        public async Task<UserSubscriptionResponse> GetSubscriptionByIdAsync(int id)
        {
            var subscription = await _unitOfWork.UserSubscriptionRepository.GetByIdAsync(id);
            if (subscription == null) throw new KeyNotFoundException("Subscription not found");
            return new UserSubscriptionResponse
            {
                Id = subscription.Id,
                UserId = subscription.UserId,
                SubscriptionTypeId = subscription.SubscriptionTypeId,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsActive = subscription.IsActive,
                PaymentStatus = subscription.PaymentStatus,
                Amount = subscription.Amount,
                PaymentGatewayTransactionId = subscription.PaymentGatewayTransactionId,
                CreatedAt = subscription.CreatedAt,
                UpdatedAt = subscription.UpdatedAt
            };
        }

        public async Task<UserSubscriptionResponse> GetByPaymentGatewayTransactionIdAsync(string transactionId)
        {
            var subscription = await  _unitOfWork.UserSubscriptionRepository.GetByPaymentGatewayTransactionIdAsync(transactionId);
            if (subscription == null) throw new KeyNotFoundException("Subscription not found");
            return new UserSubscriptionResponse
            {
                Id = subscription.Id,
                UserId = subscription.UserId,
                SubscriptionTypeId = subscription.SubscriptionTypeId,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsActive = subscription.IsActive,
                PaymentStatus = subscription.PaymentStatus,
                Amount = subscription.Amount,
                PaymentGatewayTransactionId = subscription.PaymentGatewayTransactionId,
                CreatedAt = subscription.CreatedAt,
                UpdatedAt = subscription.UpdatedAt
            };
        }
        public async Task UpdateAsync(UserSubscriptionResponse subscription)
        {
            if (subscription == null)
            {
                _logger.LogWarning("Update subscription is null.");
                throw new ArgumentNullException(nameof(subscription), "Subscription cannot be null.");
            }

            var existingSubscription = await _unitOfWork.UserSubscriptionRepository.GetByIdAsync(subscription.Id);
            if (existingSubscription == null)
                throw new NotFoundException($"Subscription with ID {subscription.Id} not found.");

            // Cập nhật các trường từ subscription (giả sử các giá trị đã được thay đổi trong HandleWebhook)
            existingSubscription.PaymentStatus = subscription.PaymentStatus;
            existingSubscription.Amount = subscription.Amount;
            existingSubscription.IsActive = subscription.IsActive;
            existingSubscription.UpdatedAt = DateTime.UtcNow; // Cập nhật thời gian hiện tại (24/07/2025 08:34 AM UTC)

            try
            {
                await _unitOfWork.UserSubscriptionRepository.UpdateAsync(existingSubscription);
                await _unitOfWork.SaveChangesWithTransactionAsync();

                _logger.LogInformation("Updated subscription with ID {SubscriptionId}.", subscription.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating subscription with ID {SubscriptionId}: {Message}", subscription.Id, ex.Message);
                throw new ApplicationException("A concurrency error occurred while updating the subscription.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating subscription with ID {SubscriptionId}: {Message}", subscription.Id, ex.Message);
                throw new ApplicationException("An error occurred while updating the subscription.", ex);
            }
        }
    }
}