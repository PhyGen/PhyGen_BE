using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class SolutionsLinkRepository : GenericRepository<SolutionsLink>
    {
        private readonly teamsevenphygendbContext _context;

        public SolutionsLinkRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<SolutionsLink>?> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<SolutionsLink?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<List<SolutionsLink>?> GetBySolutionIdAsync(int solutionId)
        {
            return await _context.SolutionsLinks
                .Where(l => l.SolutionId == solutionId)
                .ToListAsync();
        }

        public async Task<int> AddAsync(SolutionsLink link)
        {
            return await CreateAsync(link);
        }

        public async Task<int> UpdateAsync(SolutionsLink link)
        {
            return await base.UpdateAsync(link);
        }

        public async Task<bool> DeleteAsync(SolutionsLink link)
        {
            return await RemoveAsync(link);
        }
    }
}
