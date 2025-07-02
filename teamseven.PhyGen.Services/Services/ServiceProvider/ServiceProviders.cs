using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Services.ChapterService;
using teamseven.PhyGen.Services.Services.GradeService;
using teamseven.PhyGen.Services.Services.LessonService;
using teamseven.PhyGen.Services.Services.QuestionsService;
using teamseven.PhyGen.Services.Services.SemesterService;
using teamseven.PhyGen.Services.Services.SolutionLinkService;
using teamseven.PhyGen.Services.Services.SolutionReportService;
using teamseven.PhyGen.Services.Services.SubscriptionTypeService;
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
        private readonly ISemesterService _semesterService;
        private readonly IUserSocialProviderService _userSocialProviderService;
        private readonly IChapterService _chapterService;
        private readonly IGradeService _gradeService;
        private readonly ISolutionLinkService _solutionLinkService;
        private readonly ISolutionReportService _solutionReportService;
        private readonly ISubscriptionTypeService _subscriptionTypeService;
        private readonly IRegisterService _registerService;
          private readonly ILessonService _lessonService;
        public ServiceProviders(
            IAuthService authService,
            ILoginService loginService,
            IUserService userService,
            IQuestionsService questionsService,
            ISemesterService semesterService,
            IUserSocialProviderService userSocialProviderService,
            IChapterService chapterService,
            IGradeService gradeService,
            ISolutionLinkService solutionLinkService,
            ISolutionReportService solutionReportService,
            ISubscriptionTypeService subscriptionTypeService,
            IRegisterService registerService,
            ILessonService lessonService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _questionsService = questionsService ?? throw new ArgumentNullException(nameof(questionsService));
            _semesterService = semesterService ?? throw new ArgumentNullException(nameof(semesterService));
            _userSocialProviderService = userSocialProviderService ?? throw new ArgumentNullException(nameof(userSocialProviderService));
            _chapterService = chapterService ?? throw new ArgumentNullException(nameof(chapterService));
            _gradeService = gradeService ?? throw new ArgumentNullException(nameof(gradeService));
            _solutionLinkService = solutionLinkService ?? throw new ArgumentNullException(nameof(solutionLinkService));
            _solutionReportService = solutionReportService ?? throw new ArgumentNullException(nameof(solutionReportService));
            _subscriptionTypeService = subscriptionTypeService ?? throw new ArgumentNullException(nameof(subscriptionTypeService));
            _registerService = registerService ?? throw new ArgumentNullException(nameof(registerService));
            _lessonService = lessonService ?? throw new ArgumentNullException(nameof(_lessonService));
        }
      

        public IAuthService AuthService => _authService;
        public ILoginService LoginService => _loginService;
        public IUserService UserService => _userService;
        public IQuestionsService QuestionsService => _questionsService;
        public ISemesterService SemesterService => _semesterService;
        public IUserSocialProviderService UserSocialProviderService => _userSocialProviderService;
        public IChapterService ChapterService => _chapterService;
        public IGradeService GradeService => _gradeService;
        public ISolutionLinkService SolutionLinkService => _solutionLinkService;
        public ISolutionReportService SolutionReportService => _solutionReportService;
        public ISubscriptionTypeService SubscriptionTypeService => _subscriptionTypeService;
        public IRegisterService RegisterService => _registerService;
        public ILessonService LessonService => _lessonService;
    }
}