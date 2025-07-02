using System;
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
        ISolutionLinkService SolutionLinkService { get; }
        ISolutionReportService SolutionReportService { get; }
        ISubscriptionTypeService SubscriptionTypeService { get; }

        IRegisterService RegisterService { get; }
        ILessonService LessonService { get; }
    }
}