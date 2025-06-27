using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Services.Services.QuestionsService
{
    public interface IQuestionsService
    {
        public Task AddQuestionAsync(QuestionDataRequest questionDataRequest);

        public Task<QuestionDataResponse> GetQuestionById(int id);

        public Task ModifyQuestionAsync(QuestionDataRequest questionDataRequest);

        public Task DeleteQuestionAsync(int id);
 
    }
}
