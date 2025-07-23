﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<(List<Question>, int)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search = null,
    string? sort = null,
    int? lessonId = null,
    string? difficultyLevel = null,
    int? chapterId = null,
    int isSort = 0,
    int createdByUserId = 0) // Default = 0 => không lọc theo user
        {
            var query = _context.Questions
                .Include(q => q.Lesson)
                .AsQueryable();

            // 🎯 Lọc theo người tạo (nếu có)
            if (createdByUserId != 0)
            {
                query = query.Where(q => q.CreatedByUserId == createdByUserId);
            }

            // 🔍 Search (bỏ dấu)
            if (!string.IsNullOrEmpty(search))
            {
                var searchNormalized = search.RemoveDiacritics().ToLower();
                query = query.Where(q =>
                    q.Content.RemoveDiacritics().ToLower().Contains(searchNormalized) ||
                    q.QuestionSource.RemoveDiacritics().ToLower().Contains(searchNormalized));
            }

            if (lessonId.HasValue)
            {
                query = query.Where(q => q.LessonId == lessonId.Value);
            }

            if (!string.IsNullOrEmpty(difficultyLevel))
            {
                query = query.Where(q => q.DifficultyLevel == difficultyLevel);
            }

            if (chapterId.HasValue)
            {
                query = query.Where(q => q.Lesson.ChapterId == chapterId.Value);
            }

            // 📊 Sắp xếp
            if (isSort == 1)
            {
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort.ToLower())
                    {
                        case "content:asc":
                            query = query.OrderBy(q => q.Content);
                            break;
                        case "content:desc":
                            query = query.OrderByDescending(q => q.Content);
                            break;
                        case "difficultylevel:asc":
                            query = query.OrderBy(q => q.DifficultyLevel);
                            break;
                        case "difficultylevel:desc":
                            query = query.OrderByDescending(q => q.DifficultyLevel);
                            break;
                        case "createdat:asc":
                            query = query.OrderBy(q => q.CreatedAt);
                            break;
                        case "createdat:desc":
                            query = query.OrderByDescending(q => q.CreatedAt);
                            break;
                        case "updatedat:asc":
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                        case "updatedat:desc":
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                        default:
                            query = query.OrderByDescending(q => q.CreatedAt);
                            break;
                    }
                }
                else
                {
                    query = query.OrderByDescending(q => q.CreatedAt);
                }
            }
            else
            {
                query = query.OrderBy(q => q.Id);
            }

            // 📄 Pagination
            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }
    }
    }