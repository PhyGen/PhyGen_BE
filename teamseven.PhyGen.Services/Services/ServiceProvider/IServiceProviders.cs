using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Interfaces;

namespace teamseven.PhyGen.Services.Services.ServiceProvider
{
    public interface IServiceProviders
    {
        IAuthService AuthService { get; }
        ILoginService LoginService { get; }
    }
}
