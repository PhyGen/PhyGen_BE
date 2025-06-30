using System.Collections.Generic;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;

namespace teamseven.PhyGen.Services.Services.SubscriptionTypeService
{
    public interface ISubscriptionTypeService
    {
        Task<List<SubscriptionTypeResponse>> GetAllAsync();
        Task<SubscriptionTypeResponse> GetByIdAsync(int id);
        Task AddAsync(SubscriptionTypeRequest request);
        Task UpdateAsync(int id, SubscriptionTypeRequest request);
        Task DeleteAsync(int id);
    }
}
