using Microsoft.AspNetCore.Http;

public class SolutionWithVideoRequest
{
    public int SolutionId { get; set; }
    public string Content { get; set; } // Nếu muốn cập nhật nội dung
    public string? VideoData { get; set; } // nội dung phụ (mô tả video)
    public IFormFile VideoFile { get; set; }
}
