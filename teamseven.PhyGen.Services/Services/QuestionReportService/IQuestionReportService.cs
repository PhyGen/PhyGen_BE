using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Services.Services.QuestionReportService
{
    public interface IQuestionReportService
    {
        Task<IEnumerable<QuestionReport>> GetAllReportsAsync();
        Task<QuestionReport> GetReportByIdAsync(int id);
        Task CreateReportAsync(CreateQuestionReportRequest request);
        Task UpdateReportAsync(UpdateQuestionReportRequest request);
    }
}
