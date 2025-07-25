using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class SolutionWithVideoRequest
{
    public int SolutionId { get; set; }
    public string? Content { get; set; }

    [Required]
    public IFormFile VideoFile { get; set; }
}
