﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class GradeRepository : GenericRepository<Grade>
    {
        private readonly teamsevenphygendbContext _context;

        public GradeRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<Grade>?> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Grade?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<int> AddAsync(Grade grade)
        {
            return await CreateAsync(grade);
        }

        public async Task<int> UpdateAsync(Grade grade)
        {
            return await base.UpdateAsync(grade);
        }

        public async Task<bool> DeleteAsync(Grade grade)
        {
            return await RemoveAsync(grade);
        }
    }
}
