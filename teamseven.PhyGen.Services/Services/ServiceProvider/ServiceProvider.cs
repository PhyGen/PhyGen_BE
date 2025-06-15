using System;
using teamseven.PhyGen.Services.Interfaces;

namespace teamseven.PhyGen.Services.Services.ServiceProvider
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly IAuthService _authService;
        private readonly ILoginService _loginService;

        public ServiceProvider(IAuthService authService, ILoginService loginService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        public IAuthService AuthService => _authService;
        public ILoginService LoginService => _loginService;
    }
}
