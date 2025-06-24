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
    public class RoleRepository : GenericRepository<Role>
    {
        private readonly teamsevenphygendbContext _context;

        public RoleRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>?> GetRoleList()
        {
            return await GetAllAsync();
        }

        public async Task<Role?> GetByName(string name)
       {
        return await _context.Roles.FirstOrDefaultAsync(a => a.RoleName.Contains(name, StringComparison.OrdinalIgnoreCase));
       }

        public async Task<int> AddRoleAsync(Role role)
        {
            return await CreateAsync(role);
        }
        public async Task<int> UpdateRoleAsync(Role role)
        {
           return await UpdateAsync(role);
        }
        public async Task<bool> DeleteRoleAsync(Role role)
        {
            return await RemoveAsync(role);
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await GetByIdAsync(id);
        }
    }


}
