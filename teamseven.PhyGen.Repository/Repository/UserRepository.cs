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

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        //public async Task DeleteUserAsync(int id) { var user = await _context.Users.FindAsync(id); if (user != null) { _context.Users.Remove(user); await _context.SaveChangesAsync(); } }

        public async Task<int> SoftDeleteUserAsync(int id)
        {
            var user = await GetByIdAsync(id); 
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            else
            if ((bool)!user.IsActive)
            {
                return 0; 
            }

            user.IsActive = false;
            return await UpdateAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await UpdateAsync(user);
        }

        public async Task<int> ChangeUserRoleAsync(int userId, string role)
        {
            //var validRoles = new[] { "student", "staff", "admin" };
            //if (!validRoles.Contains(role))
            //{
            //    throw new ArgumentException($"Invalid role. Role must be one of: {string.Join(", ", validRoles)}.");
            //}

            // Lấy user
            var user = await GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            if (user.Role == role)
            {
                return 0; 
            }
            user.Role = role;
            return await UpdateAsync(user);
        }

    }
}
