using System;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Repository.Repository;

namespace teamseven.PhyGen.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        UserRepository UserRepository { get; }
        UserSocialProviderRepository UserSocialProvider { get; }
        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly teamsevenphygendbContext _context;
        private UserRepository _userRepository;
        private UserSocialProviderRepository _userSocialProvider;
        private bool _disposed = false;

        public UnitOfWork(teamsevenphygendbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }
        public UserSocialProviderRepository UserSocialProvider
        {
            get
            {
                return _userSocialProvider ??= new UserSocialProviderRepository(_context);
            }
        }

        public int SaveChangesWithTransaction()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                int result = _context.SaveChanges();
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
      
        public async Task<int> SaveChangesWithTransactionAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                //_userRepository?.Dispose();
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
