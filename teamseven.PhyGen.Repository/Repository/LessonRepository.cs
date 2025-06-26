using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class LessonRepository : GenericRepository<Lesson>
    {
        private readonly teamsevenphygendbContext _context;

        public LessonRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<Lesson>?> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<List<Lesson>?> GetByChapterIdAsync(int chapterId)
        {
            return await _context.Lessons
                .Where(l => l.ChapterId == chapterId)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Lesson lesson)
        {
            return await CreateAsync(lesson);
        }

        public async Task<int> UpdateAsync(Lesson lesson)
        {
            return await base.UpdateAsync(lesson);
        }

        public async Task<bool> DeleteAsync(Lesson lesson)
        {
            return await RemoveAsync(lesson);
        }
    }
}
