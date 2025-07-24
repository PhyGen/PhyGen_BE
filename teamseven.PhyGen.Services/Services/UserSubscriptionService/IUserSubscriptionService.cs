using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Interfaces
{
    public interface IUserSubscriptionService
    {
        Task AddSubscriptionAsync(UserSubscriptionRequest request);
        Task UpdateAsync(UserSubscriptionResponse subscription);
        Task<UserSubscriptionResponse> GetSubscriptionByIdAsync(int id);
        Task<UserSubscriptionResponse> GetByPaymentGatewayTransactionIdAsync(string transactionId);
    }
}