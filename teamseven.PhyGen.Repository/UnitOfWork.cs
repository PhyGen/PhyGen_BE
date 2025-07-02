﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Repository.Repository;

namespace teamseven.PhyGen.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ChapterRepository ChapterRepository { get; }
        ExamQuestionRepository ExamQuestionRepository { get; }
        ExamRepository ExamRepository { get; }
        ExamTypeRepository ExamTypeRepository { get; }
        GradeRepository GradeRepository { get; }
        LessonRepository LessonRepository { get; }
        QuestionRepository QuestionRepository { get; }
        RoleRepository RoleRepository { get; }
        SemesterRepository SemesterRepository { get; }
        SolutionReportRepository SolutionReportRepository { get; }
        SolutionRepository SolutionRepository { get; }
        SolutionsLinkRepository SolutionsLinkRepository { get; }
        SubscriptionTypeRepository SubscriptionTypeRepository { get; }
        UserRepository UserRepository { get; }
        UserSubscriptionRepository UserSubscriptionRepository { get; }
        UserSocialProviderRepository UserSocialProviderRepository { get; }

        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly teamsevenphygendbContext _context;

        private ChapterRepository _chapterRepository;
        private ExamQuestionRepository _examQuestionRepository;
        private ExamRepository _examRepository;
        private ExamTypeRepository _examTypeRepository;
        private GradeRepository _gradeRepository;
        private LessonRepository _lessonRepository;
        private QuestionRepository _questionRepository;
        private RoleRepository _roleRepository;
        private SemesterRepository _semesterRepository;
        private SolutionReportRepository _solutionReportRepository;
        private SolutionRepository _solutionRepository;
        private SolutionsLinkRepository _solutionsLinkRepository;
        private SubscriptionTypeRepository _subscriptionTypeRepository;
        private UserRepository _userRepository;
        private UserSubscriptionRepository _userSubscriptionRepository;
        private UserSocialProviderRepository _userSocialProvider;

        private bool _disposed = false;

        public UnitOfWork(teamsevenphygendbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ChapterRepository ChapterRepository => _chapterRepository ??= new ChapterRepository(_context);
        public ExamQuestionRepository ExamQuestionRepository => _examQuestionRepository ??= new ExamQuestionRepository(_context);
        public ExamRepository ExamRepository => _examRepository ??= new ExamRepository(_context);
        public ExamTypeRepository ExamTypeRepository => _examTypeRepository ??= new ExamTypeRepository(_context);
        public GradeRepository GradeRepository => _gradeRepository ??= new GradeRepository(_context);
        public LessonRepository LessonRepository => _lessonRepository ??= new LessonRepository(_context);
        public QuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(_context);
        public RoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context);
        public SemesterRepository SemesterRepository => _semesterRepository ??= new SemesterRepository(_context);
        public SolutionReportRepository SolutionReportRepository => _solutionReportRepository ??= new SolutionReportRepository(_context);
        public SolutionRepository SolutionRepository => _solutionRepository ??= new SolutionRepository(_context);
        public SolutionsLinkRepository SolutionsLinkRepository => _solutionsLinkRepository ??= new SolutionsLinkRepository(_context);
        public SubscriptionTypeRepository SubscriptionTypeRepository => _subscriptionTypeRepository ??= new SubscriptionTypeRepository(_context);
        public UserRepository UserRepository => _userRepository ??= new UserRepository(_context);
        public UserSubscriptionRepository UserSubscriptionRepository => _userSubscriptionRepository ??= new UserSubscriptionRepository(_context);
        public UserSocialProviderRepository UserSocialProviderRepository => _userSocialProvider ??= new UserSocialProviderRepository(_context);

        public int SaveChangesWithTransaction()
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            int result = 0;
            strategy.Execute(() =>
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    result = _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            });
            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            int result = 0;
            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _context?.Dispose();
            }

            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}