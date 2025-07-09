using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class QuestionReportRepository : GenericRepository<QuestionReport>
    {
        private readonly teamsevenphygendbContext _context;

        public QuestionReportRepository(teamsevenphygendbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<QuestionReport>> GetAllReportsAsync()
        {
            return await _context.QuestionReports.ToListAsync();
        }

        public async Task<QuestionReport?> GetReportByIdAsync(int id)
        {
            return await _context.QuestionReports.FindAsync(id);
        }

        public async Task CreateReportAsync(QuestionReport report)
        {
            _context.QuestionReports.Add(report);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReportAsync(QuestionReport report)
        {
            _context.QuestionReports.Update(report);
            await _context.SaveChangesAsync();
        }

    }
}
