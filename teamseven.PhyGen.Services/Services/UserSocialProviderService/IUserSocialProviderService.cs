using System.Collections.Generic;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.UserSocialProviderService
{
    public interface IUserSocialProviderService
    {
        Task AddAsync(UserSocialProviderRequest request);
        Task<UserSocialProviderResponse> GetByIdAsync(int id);
        Task<List<UserSocialProviderResponse>> GetByUserIdAsync(int userId);
        Task UpdateAsync(UserSocialProviderRequest request);
        Task DeleteAsync(int id);
    }
}
