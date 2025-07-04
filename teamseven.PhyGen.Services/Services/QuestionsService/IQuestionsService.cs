using System.Collections.Generic;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.QuestionsService
{
    public interface IQuestionsService
    {
        Task AddQuestionAsync(QuestionDataRequest questionDataRequest);
        Task DeleteQuestionAsync(int id);
        Task<QuestionDataResponse> GetQuestionById(int id);
        Task ModifyQuestionAsync(QuestionDataRequest questionDataRequest);
        Task<PagedResponse<QuestionDataResponse>> GetQuestionsAsync(
            int? pageNumber = null,
            int? pageSize = null,
            string? search = null,
            string? sort = null,
            int? lessonId = null,
            string? difficultyLevel = null,
            int? chapterId = null,
            int isSort = 0);
    }
}