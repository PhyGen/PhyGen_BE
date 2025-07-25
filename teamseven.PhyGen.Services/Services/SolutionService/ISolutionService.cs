using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.SolutionService
{
    public interface ISolutionService
    {
        Task<IEnumerable<SolutionDataResponse>> GetAllSolutionsAsync();
        Task<SolutionDataResponse> GetSolutionByIdAsync(int id);
        Task CreateSolutionAsync(CreateSolutionRequest request);
        Task UpdateSolutionAsync(SolutionDataRequest request);
        Task DeleteSolutionAsync(int id);
        Task AddSolutionWithVideoAsync(SolutionWithVideoRequest request);
        Task UpdateSolutionVideoAsync(SolutionWithVideoRequest request);
    }
}
