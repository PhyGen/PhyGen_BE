using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Services.Interfaces
{
    public interface ILoginService
    {
        Task<string> ValidateUserAsync(string email, string password);
    }
}
