using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Repository.Repository.Interfaces;

namespace teamseven.PhyGen.Repository.Repository
{
    public class UserSocialProviderRepository : IUserSocialProviderRepository
    {
        private readonly teamsevenphygendbContext _context;

        public UserSocialProviderRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<UserSocialProvider> GetByProviderAsync(string providerName, string providerId)
        {
            return await _context.UserSocialProviders
                .FirstOrDefaultAsync(usp => usp.ProviderName == providerName && usp.ProviderId == providerId)
                ?? new UserSocialProvider();
        }

        public async Task<UserSocialProvider> GetByIdAsync(int id)
        {
            return await _context.UserSocialProviders.FindAsync(id);
        }

        public async Task<List<UserSocialProvider>> GetByUserIdAsync(int userId)
        {
            return await _context.UserSocialProviders
                .Where(usp => usp.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(UserSocialProvider userSocialProvider)
        {
            await _context.UserSocialProviders.AddAsync(userSocialProvider);
        }

        public Task UpdateAsync(UserSocialProvider userSocialProvider)
        {
            _context.UserSocialProviders.Update(userSocialProvider);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.UserSocialProviders.Remove(entity);
            }
        }
    }
}
