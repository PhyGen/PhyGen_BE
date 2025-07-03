using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.UserSocialProviderService
{
    public interface IUserSocialProviderService
    {
        Task<IEnumerable<UserSocialProviderDataResponse>> GetAllAsync();
        Task<UserSocialProviderDataResponse> GetByIdAsync(int id);
        Task CreateAsync(CreateUserSocialProviderRequest request);
        Task UpdateAsync(UserSocialProviderDataRequest request);
        Task DeleteAsync(int id);
    }
}
