using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace teamseven.PhyGen.Repository.Dtos;

public class QuestionDataRequest
{
    [Required(ErrorMessage = "Content is required.")]
    [StringLength(5000, ErrorMessage = "Content cannot exceed 5000 characters.")]
    public string Content { get; set; }

    [StringLength(500, ErrorMessage = "Question source cannot exceed 500 characters.")]
    public string QuestionSource { get; set; }

    [Required(ErrorMessage = "Difficulty level is required.")]
    [RegularExpression("^(Easy|Medium|Hard)$", ErrorMessage = "Difficulty level must be Easy, Medium, or Hard.")]
    public string DifficultyLevel { get; set; }

    [Required(ErrorMessage = "Lesson ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lesson ID must be a positive integer.")]
    public int LessonId { get; set; }

    [Required(ErrorMessage = "Created by user ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Created by user ID must be a positive integer.")]
    public int CreatedByUserId { get; set; }
}