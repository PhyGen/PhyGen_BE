using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamSeven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Services.Interfaces
{
    public interface ILoginService
    {
     Task<(bool IsSuccess, string ResultOrError)> ValidateUserAsync(TeamSeven.PhyGen.Services.Object.Requests.LoginRequest loginRequest);
    }
}
