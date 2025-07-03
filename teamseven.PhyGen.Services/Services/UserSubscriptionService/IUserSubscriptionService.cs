using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.UserSubscriptionService
{
    public interface IUserSubscriptionService
    {
        Task<IEnumerable<UserSubscriptionDataResponse>> GetAllAsync();
        Task<UserSubscriptionDataResponse> GetByIdAsync(int id);
        Task CreateAsync(CreateUserSubscriptionRequest request);
        Task UpdateAsync(UserSubscriptionDataRequest request);
        Task DeleteAsync(int id);
    }
}
