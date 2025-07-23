using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class ExamQuestionRepository : GenericRepository<ExamQuestion>
    {
        private readonly teamsevenphygendbContext _context;

        public ExamQuestionRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<ExamQuestion>?> GetByExamIdAsync(int examId)
        {
            return await _context.ExamQuestions
                .Where(eq => eq.ExamId == examId)
                .Include(eq => eq.Exam)     // Tải Exam để lấy ExamName
                .Include(eq => eq.Question) // Tải Question để lấy QuestionContent
                .ToListAsync();
        }

        public async Task<List<ExamQuestion>?> GetByQuestionIdAsync(int questionId)
        {
            return await _context.ExamQuestions
                .Where(eq => eq.QuestionId == questionId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Exam>> GetByCreatorAsync(int userId)
        {
            return await _context.Exams
                .Where(e => e.CreatedByUserId == userId && !e.IsDeleted) // Lọc theo userId và không bị xóa
                .Include(e => e.Lesson)        // Tải Lesson để lấy LessonName
                .Include(e => e.ExamType)      // Tải ExamType để lấy ExamTypeName
                .Include(e => e.CreatedByUser) // Tải CreatedByUser để lấy Name hoặc Email
                .ToListAsync();
        }
        public async Task<int> AddAsync(ExamQuestion examQuestion)
        {
            return await CreateAsync(examQuestion);
        }

        public async Task<int> UpdateAsync(ExamQuestion examQuestion)
        {
            return await base.UpdateAsync(examQuestion);
        }

        public async Task<bool> DeleteAsync(ExamQuestion examQuestion)
        {
            return await RemoveAsync(examQuestion);
        }

        public async Task<ExamQuestion?> GetByIdAsync(int examId)
        {
            return await base.GetByIdAsync(examId);
        }

        public async Task<ExamQuestion?> GetByExamAndQuestionIdAsync(int examId, int questionId)
        {
            return await _context.ExamQuestions
                .FirstOrDefaultAsync(eq => eq.ExamId == examId && eq.QuestionId == questionId);
        }
    }
}
