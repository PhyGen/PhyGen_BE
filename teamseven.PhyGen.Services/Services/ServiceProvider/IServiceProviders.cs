using System;
using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Services.ChapterService;
using teamseven.PhyGen.Services.Services.GradeService;
using teamseven.PhyGen.Services.Services.LessonService;
using teamseven.PhyGen.Services.Services.QuestionsService;
using teamseven.PhyGen.Services.Services.SemesterService;
using teamseven.PhyGen.Services.Services.SolutionReportService;
using teamseven.PhyGen.Services.Services.SubscriptionTypeService;
using teamseven.PhyGen.Services.Services.UserService;
using teamseven.PhyGen.Services.Services.UserSocialProviderService;
using teamseven.PhyGen.Services.Services.SolutionService;
using teamseven.PhyGen.Services.Services.UserSubscriptionService;
using teamseven.PhyGen.Services.Services.ExamService;
using teamseven.PhyGen.Services.Services.QuestionReportService;
using teamseven.PhyGen.Services.Services.TextBookService;

namespace teamseven.PhyGen.Services.Services.ServiceProvider
{
    public interface IServiceProviders
    {
        IAuthService AuthService { get; }
        ILoginService LoginService { get; }
        IUserService UserService { get; }
        IQuestionsService QuestionsService { get; }
        ISemesterService SemesterService { get; }
        IUserSocialProviderService UserSocialProviderService { get; }
        IChapterService ChapterService { get; }
        IGradeService GradeService { get; }
        ISolutionReportService SolutionReportService { get; }
        ISubscriptionTypeService SubscriptionTypeService { get; }

        IRegisterService RegisterService { get; }
        ILessonService LessonService { get; }
        ISolutionService SolutionService { get; }

        IUserSubscriptionService UserSubscriptionService { get; }

        IExamService ExamService { get; }

        IQuestionReportService QuestionReportService { get; }

        ITextBookService TextBookService { get; }

        IPayOSService PayOSService { get; }
    }
}