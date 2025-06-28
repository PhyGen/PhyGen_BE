using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Services.Services.ChapterService
{
    public interface IChapterService
    {
        Task CreateChapterAsync(CreateChapterRequest request);
        Task DeleteChapterAsync(int id);
    }
}
