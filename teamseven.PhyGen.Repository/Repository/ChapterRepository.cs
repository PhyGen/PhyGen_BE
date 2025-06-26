﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class ChapterRepository : GenericRepository<Chapter>
    {
        private readonly teamsevenphygendbContext _context;

        public ChapterRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<Chapter>?> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Chapter?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<List<Chapter>?> GetBySemesterIdAsync(int semesterId)
        {
            return await _context.Chapters
                .Where(c => c.SemesterId == semesterId)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Chapter chapter)
        {
            return await CreateAsync(chapter);
        }

        public async Task<int> UpdateAsync(Chapter chapter)
        {
            return await base.UpdateAsync(chapter);
        }

        public async Task<bool> DeleteAsync(Chapter chapter)
        {
            return await RemoveAsync(chapter);
        }
    }
}
