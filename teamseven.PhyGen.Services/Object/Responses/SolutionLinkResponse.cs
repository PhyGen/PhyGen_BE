using System;

namespace teamseven.PhyGen.Repository.Dtos
{
    public class SolutionLinkResponse
    {
        public int Id { get; set; }
        public int SolutionId { get; set; }
        public string Link { get; set; }
        public string GeneratedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
