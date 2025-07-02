using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.GradeService
{
    public interface IGradeService
    {
        Task<IEnumerable<GradeDataResponse>> GetAllGradesAsync();
        Task<GradeDataResponse> GetGradeByIdAsync(int id);
        Task CreateGradeAsync(CreateGradeRequest request);
        Task UpdateGradeAsync(GradeDataRequest request);
        Task DeleteGradeAsync(int id);
    }
}