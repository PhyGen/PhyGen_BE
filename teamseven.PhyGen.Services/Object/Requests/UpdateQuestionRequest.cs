using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Services.Object.Requests
{
    public class UpdateQuestionRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(5000, ErrorMessage = "Content cannot exceed 5000 characters.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "DifficultyLevel is required.")]
        public string DifficultyLevel { get; set; }
    }
}
