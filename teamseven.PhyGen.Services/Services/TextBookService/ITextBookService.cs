using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.TextBookService
{
    public interface ITextBookService
    {
        Task<IEnumerable<TextBookDataResponse>> GetAllTextBookAsync();
        Task<TextBookDataResponse> GetTextBookByIdAsync(int id);
        Task CreateTextBookAsync(CreateTextBookRequest request);
        Task UpdateTextBookAsync(TextBookDataRequest request);
        Task DeleteTextBookAsync(int id);
    }
}
