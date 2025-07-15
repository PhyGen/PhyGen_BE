﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class UserRepository: GenericRepository<User>
    {
        private readonly teamsevenphygendbContext _context;

        public UserRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email) { return await _context.Users.FirstOrDefaultAsync(u => u.Email == email); }

        public async Task<IEnumerable<User>?> GetAllUserAsync()
        {
            return await GetAllAsync();
        }

        public async Task<int> SetLoginDateTime(int id)
        {
            var user = await GetByIdAsync(id) ?? throw new ArgumentNullException(nameof(id));

            user.LastLoginAt = DateTime.UtcNow;

            Console.WriteLine("LOG: UPDATED LOGIN DATE TIME " + user.LastLoginAt + user.Id);

            return await _context.SaveChangesAsync();

        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task AddQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }
        //public async Task DeleteUserAsync(int id) { var user = await _context.Users.FindAsync(id); if (user != null) { _context.Users.Remove(user); await _context.SaveChangesAsync(); } }

        public async Task<User> SoftDeleteUserAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            if (!user.IsActive.HasValue || !user.IsActive.Value)
            {
                return null; 
            }

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            return user; 
        }

        public async Task UpdateUserAsync(User user)
        {
            await UpdateAsync(user);
        }
        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        private static readonly Dictionary<string, int> ValidRoles = new()
        {
            { "user", 1 },
            { "admin", 2 },
            { "moderator", 3 }
        };

        public async Task<(bool IsSuccess, string ResultOrError)> ChangeUserRoleAsync(int userId, string role)
        {
            // check hop le
            if (string.IsNullOrEmpty(role) || !ValidRoles.ContainsKey(role.ToLower()))
            {
                return (false, $"Invalid role. Role must be one of: {string.Join(", ", ValidRoles.Keys)}");
            }

            // get user
            var user = await GetByIdAsync(userId);
            if (user == null)
            {
                return (false, $"User with ID {userId} not found");
            }

            // check user role
            int newRoleId = ValidRoles[role.ToLower()];
            if (user.RoleId == newRoleId)
            {
                return (true, "User already has this role");
            }

            // update new role
            user.RoleId = newRoleId;
            user.UpdatedAt = DateTime.UtcNow;
            int affectedRows = await UpdateAsync(user);

            if (affectedRows == 0)
            {
                return (false, "Failed to update user role");
            }

            return (true, "Role changed successfully");
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
