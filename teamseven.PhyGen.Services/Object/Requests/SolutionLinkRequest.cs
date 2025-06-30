using System.ComponentModel.DataAnnotations;

namespace teamseven.PhyGen.Repository.Dtos
{
    public class SolutionLinkRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "SolutionId must be a positive number.")]
        public int SolutionId { get; set; }

        [StringLength(500)]
        public string Link { get; set; }

        [StringLength(50)]
        public string GeneratedBy { get; set; }
    }
}
