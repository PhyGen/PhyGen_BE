using System;
using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Services.UserService;

namespace teamseven.PhyGen.Services.Services.ServiceProvider
{
    public class ServiceProviders : IServiceProviders
    {
        private readonly IAuthService _authService;
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;

        public ServiceProviders(IAuthService authService, ILoginService loginService, IUserService userService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _userService = userService ?? throw new ArgumentNullException( nameof(userService));
        }

        public IAuthService AuthService => _authService;
        public ILoginService LoginService => _loginService;

        public IUserService UserService => _userService;
    }
}
