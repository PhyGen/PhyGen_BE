using System;
using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Services.QuestionsService;
using teamseven.PhyGen.Services.Services.UserService;
using teamseven.PhyGen.Services.Services.UserSocialProviderService;

namespace teamseven.PhyGen.Services.Services.ServiceProvider
{
    public class ServiceProviders : IServiceProviders
    {
        private readonly IAuthService _authService;
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;
        private readonly IQuestionsService _questionsService;
        private readonly IUserSocialProviderService _userSocialProviderService;

        public ServiceProviders(IAuthService authService, ILoginService loginService, IUserService userService, IQuestionsService questions, IUserSocialProviderService usersocial)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _userService = userService ?? throw new ArgumentNullException( nameof(userService));
            _questionsService = questions ?? throw new ArgumentNullException(nameof(_questionsService));
            _userSocialProviderService = usersocial ?? throw new ArgumentNullException(nameof(_userSocialProviderService));
        }

        public IAuthService AuthService => _authService;
        public ILoginService LoginService => _loginService;

        public IUserService UserService => _userService;

        public IQuestionsService QuestionsService => _questionsService;

        public IUserSocialProviderService UserSocialProviderService => _userSocialProviderService;
    }
}
