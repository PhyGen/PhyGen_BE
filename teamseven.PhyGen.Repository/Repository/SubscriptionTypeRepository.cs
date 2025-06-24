using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class SubscriptionTypeRepository: GenericRepository<SubscriptionType>
    {
        private readonly UserRepository _userRepository;

        public SubscriptionTypeRepository(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> CreateSubcritionTypeAsync(SubscriptionType subcritionType)
        {
            return await CreateAsync(subcritionType);
        }
    }
}
