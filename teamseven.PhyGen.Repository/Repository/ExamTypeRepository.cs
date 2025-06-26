﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class ExamTypeRepository : GenericRepository<ExamType>
    {
        private readonly teamsevenphygendbContext _context;

        public ExamTypeRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<ExamType>?> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<ExamType?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<ExamType?> GetByNameAsync(string name)
        {
            return await _context.ExamTypes
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<int> AddAsync(ExamType examType)
        {
            return await CreateAsync(examType);
        }

        public async Task<int> UpdateAsync(ExamType examType)
        {
            return await base.UpdateAsync(examType);
        }

        public async Task<bool> DeleteAsync(ExamType examType)
        {
            return await RemoveAsync(examType);
        }
    }
}
