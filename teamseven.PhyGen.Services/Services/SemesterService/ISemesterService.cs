using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Services.Services.SemesterService
{
    public interface ISemesterService
    {
        Task CreateSemesterAsync(CreateSemesterRequest request);
        Task DeleteSemesterAsync(int id);
    }
}
