﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class UserSubscriptionRepository : GenericRepository<UserSubscription>
    {
        private readonly teamsevenphygendbContext _context;

        public UserSubscriptionRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<UserSubscription>?> GetAllSubscriptionsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<UserSubscription?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<List<UserSubscription>?> GetByUserIdAsync(long userId)
        {
            return await _context.UserSubscriptions
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        public async Task<UserSubscription> GetByPaymentGatewayTransactionIdAsync(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                throw new ArgumentNullException(nameof(transactionId), "PaymentGatewayTransactionId cannot be null or empty.");
            }

            return await _context.UserSubscriptions
                .FirstOrDefaultAsync(us => us.PaymentGatewayTransactionId == transactionId);
        }
        public async Task<List<UserSubscription>?> GetActiveSubscriptionsAsync(long userId)
        {
            return await _context.UserSubscriptions
                .Where(x => x.UserId == userId && x.IsActive)
                .ToListAsync();
        }

        public async Task<int> AddAsync(UserSubscription subscription)
        {
            return await CreateAsync(subscription);
        }

        public async Task<int> UpdateAsync(UserSubscription subscription)
        {
            return await UpdateAsync(subscription);
        }

        public async Task<bool> DeleteAsync(UserSubscription subscription)
        {
            return await RemoveAsync(subscription);
        }
    }
}
