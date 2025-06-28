using System;
using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Services.ChapterService;
using teamseven.PhyGen.Services.Services.GradeService;
using teamseven.PhyGen.Services.Services.QuestionsService;
using teamseven.PhyGen.Services.Services.SemesterService;
using teamseven.PhyGen.Services.Services.UserService;

namespace teamseven.PhyGen.Services.Services.ServiceProvider
{
    public class ServiceProviders : IServiceProviders
    {
        private readonly IAuthService _authService;
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;
        private readonly IQuestionsService _questionsService;
        private readonly IGradeService _gradeService;
        private readonly IChapterService _chapterService;
        public readonly ISemesterService _semesterService;

        public ServiceProviders(IAuthService authService, ILoginService loginService, IUserService userService, IQuestionsService questions,
            IGradeService gradeService, IChapterService chapterService, ISemesterService semesterService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _userService = userService ?? throw new ArgumentNullException( nameof(userService));
            _questionsService = questions ?? throw new ArgumentNullException(nameof(_questionsService));
            _gradeService = gradeService ?? throw new ArgumentNullException(nameof(gradeService));
            _chapterService = chapterService ?? throw new ArgumentNullException(nameof(chapterService));
            _semesterService = semesterService ?? throw new ArgumentNullException(nameof(semesterService));
        }

        public IAuthService AuthService => _authService;
        public ILoginService LoginService => _loginService;

        public IUserService UserService => _userService;

        public IQuestionsService QuestionsService => _questionsService;

        public IGradeService GradeService => _gradeService;

        public IChapterService ChapterService => _chapterService;

        public ISemesterService SemesterService => _semesterService;
    }
}
