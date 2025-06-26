using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class SemesterRepository : GenericRepository<Semester>
    {
        private readonly teamsevenphygendbContext _context;

        public SemesterRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<Semester>?> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Semester?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<List<Semester>?> GetByGradeIdAsync(int gradeId)
        {
            return await _context.Semesters
                .Where(s => s.GradeId == gradeId)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Semester semester)
        {
            return await CreateAsync(semester);
        }

        public async Task<int> UpdateAsync(Semester semester)
        {
            return await base.UpdateAsync(semester);
        }

        public async Task<bool> DeleteAsync(Semester semester)
        {
            return await RemoveAsync(semester);
        }
    }
}
