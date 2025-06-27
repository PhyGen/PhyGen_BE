using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .FirstOrDefaultAsync(usp => usp.ProviderName == providerName && usp.ProviderId == providerId) ?? new UserSocialProvider();
        }

        public async Task AddAsync(UserSocialProvider userSocialProvider)
        {
            await _context.UserSocialProviders.AddAsync(userSocialProvider);
        }
    }
}
