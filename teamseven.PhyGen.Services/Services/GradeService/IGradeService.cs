using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Services.Services.GradeService
{
    public interface IGradeService
    {
        Task CreateGradeAsync(CreateGradeRequest request);
        Task DeleteGradeAsync(int id);
    }
}