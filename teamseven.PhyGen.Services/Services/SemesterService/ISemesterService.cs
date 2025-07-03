using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.SemesterService
{
    public interface ISemesterService
    {
        Task<IEnumerable<SemesterDataResponse>> GetAllSemesterAsync();
        Task<SemesterDataResponse> GetSemesterByIdAsync(int id);
        Task CreateSemesterAsync(CreateSemesterRequest request);
        Task UpdateSemesterAsync(SemesterDataRequest request);
        Task DeleteSemesterAsync(int id);
    }
}
