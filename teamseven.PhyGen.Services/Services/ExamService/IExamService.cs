using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.ExamService
{
    public interface IExamService
    {
        Task CreateExamAsync(ExamRequest examRequest);

        Task<IEnumerable<ExamResponse>> GetAllExamAsync();

        Task<ExamResponse> GetExamAsync(int id);

        Task CreateExamQuestionAsync(ExamQuestionRequest examQuestionRequest);

        Task<IEnumerable<ExamResponse>> GetExamsByUserIdAsync(int userId);

        Task<IEnumerable<ExamQuestionResponse>> GetExamQuestionByIdAsync(int id);

        Task RemoveExamQuestion(ExamQuestionRequest examQuestionRequest);

        Task CreateExamHistoryAsync(ExamHistoryRequest examHistoryRequest);

        Task DeleteExamHistoryAsync(ExamHistoryRequest historyRequest);

        Task<ExamHistoryResponseDto> GetExamHistoryResponseAsync(int id);
    }
}
