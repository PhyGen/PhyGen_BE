using System;

namespace teamseven.PhyGen.Repository.Dtos
{
    public class UserSocialProviderResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ProviderName { get; set; }

        public string ProviderId { get; set; }

        public string Email { get; set; }

        public string ProfileUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // related info
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
    }
}
