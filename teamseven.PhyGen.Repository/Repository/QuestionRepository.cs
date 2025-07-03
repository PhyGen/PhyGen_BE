using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class QuestionRepository : GenericRepository<Question>
    {
        private readonly teamsevenphygendbContext _context;

        public QuestionRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<Question>?> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<List<Question>?> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Questions
                .Where(q => q.LessonId == lessonId)
                .ToListAsync();
        }

        public async Task<List<Question>?> GetByCreatorAsync(long userId)
        {
            return await _context.Questions
                .Where(q => q.CreatedByUserId == userId)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Question question)
        {
            return await CreateAsync(question);
        }

        public async Task<int> UpdateAsync(Question question)
        {
            return await base.UpdateAsync(question);
        }

        public async Task<bool> DeleteAsync(Question question)
        {
            return await RemoveAsync(question);
        }

        public async Task<(List<Question>, int)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Questions.AsQueryable();
            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalItems);
        }
    }
}
