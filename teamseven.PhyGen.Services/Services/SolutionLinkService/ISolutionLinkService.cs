using System.Collections.Generic;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;

namespace teamseven.PhyGen.Services.Services.SolutionLinkService
{
    public interface ISolutionLinkService
    {
        Task AddAsync(SolutionLinkRequest request);
        Task<SolutionLinkResponse> GetByIdAsync(int id);
        Task<List<SolutionLinkResponse>> GetBySolutionIdAsync(int solutionId);
        Task UpdateAsync(int id, SolutionLinkRequest request);
        Task DeleteAsync(int id);
    }
}
