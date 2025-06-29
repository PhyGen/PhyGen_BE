using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Services.Services.LessonService
{
    public interface ILessonService
    {
        Task CreateLessonAsync(CreateLessonRequest request);
        Task DeleteLessonAsync(int id);
    }
}
