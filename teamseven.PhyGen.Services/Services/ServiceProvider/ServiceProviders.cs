using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Services.ChapterService;
using teamseven.PhyGen.Services.Services.ExamService;
using teamseven.PhyGen.Services.Services.GradeService;
using teamseven.PhyGen.Services.Services.LessonService;
using teamseven.PhyGen.Services.Services.QuestionReportService;
using teamseven.PhyGen.Services.Services.QuestionsService;
using teamseven.PhyGen.Services.Services.SemesterService;
using teamseven.PhyGen.Services.Services.SolutionReportService;
using teamseven.PhyGen.Services.Services.SolutionService;
using teamseven.PhyGen.Services.Services.SubscriptionTypeService;
using teamseven.PhyGen.Services.Services.TextBookService;
using teamseven.PhyGen.Services.Services.UserService;
using teamseven.PhyGen.Services.Services.UserSocialProviderService;
using teamseven.PhyGen.Services.Services.UserSubscriptionService;

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
        private readonly ISolutionReportService _solutionReportService;
        private readonly ISubscriptionTypeService _subscriptionTypeService;
        private readonly IRegisterService _registerService;
        private readonly ILessonService _lessonService;
        private readonly ISolutionService _solutionService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IExamService _examService;
        private readonly IQuestionReportService _questionReportService;
        private readonly ITextBookService _textBookService;
        public ServiceProviders(
            IAuthService authService,
            ILoginService loginService,
            IUserService userService,
            IQuestionsService questionsService,
            ISemesterService semesterService,
            IUserSocialProviderService userSocialProviderService,
            IChapterService chapterService,
            IGradeService gradeService,
            ISolutionReportService solutionReportService,
            ISubscriptionTypeService subscriptionTypeService,
            IRegisterService registerService,
            ILessonService lessonService,
            ISolutionService solutionService,
            IUserSubscriptionService userSubscriptionService,
            IExamService examService,
            IQuestionReportService questionReportService,
            ITextBookService textBookService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _questionsService = questionsService ?? throw new ArgumentNullException(nameof(questionsService));
            _semesterService = semesterService ?? throw new ArgumentNullException(nameof(semesterService));
            _userSocialProviderService = userSocialProviderService ?? throw new ArgumentNullException(nameof(userSocialProviderService));
            _chapterService = chapterService ?? throw new ArgumentNullException(nameof(chapterService));
            _gradeService = gradeService ?? throw new ArgumentNullException(nameof(gradeService));
            _solutionReportService = solutionReportService ?? throw new ArgumentNullException(nameof(solutionReportService));
            _subscriptionTypeService = subscriptionTypeService ?? throw new ArgumentNullException(nameof(subscriptionTypeService));
            _registerService = registerService ?? throw new ArgumentNullException(nameof(registerService));
            _lessonService = lessonService ?? throw new ArgumentNullException(nameof(_lessonService));
            _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
            _userSubscriptionService = userSubscriptionService ?? throw new ArgumentException(nameof(userSubscriptionService));
            _examService = examService ?? throw new ArgumentNullException(nameof(examService));
            _questionReportService = questionReportService ?? throw new ArgumentNullException(nameof(questionReportService));
            _textBookService = textBookService ?? throw new ArgumentNullException(nameof(textBookService));
        }
      

        public IAuthService AuthService => _authService;
        public ILoginService LoginService => _loginService;
        public IUserService UserService => _userService;
        public IQuestionsService QuestionsService => _questionsService;
        public ISemesterService SemesterService => _semesterService;
        public IUserSocialProviderService UserSocialProviderService => _userSocialProviderService;
        public IChapterService ChapterService => _chapterService;
        public IGradeService GradeService => _gradeService;
        public ISolutionReportService SolutionReportService => _solutionReportService;
        public ISubscriptionTypeService SubscriptionTypeService => _subscriptionTypeService;
        public IRegisterService RegisterService => _registerService;
        public ILessonService LessonService => _lessonService;
        public ISolutionService SolutionService => _solutionService;
        public IUserSubscriptionService UserSubscriptionService => _userSubscriptionService;
        public IExamService ExamService => _examService;

        public IQuestionReportService QuestionReportService => _questionReportService;

        public ITextBookService TextBookService => _textBookService;
    }
}