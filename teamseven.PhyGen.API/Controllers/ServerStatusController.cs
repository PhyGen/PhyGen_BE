using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace teamseven.PhyGen.API.Controllers
{
    [ApiController] // Chỉ ra rằng đây là một API Controller
    //[Route("api/status")] // Định nghĩa route cơ bản cho Controller này. Ví dụ: /api/testserver
    public class ServerStatusController : ControllerBase // Kế thừa từ ControllerBase cho các API Controller
    {
        private readonly ILogger<ServerStatusController> _logger;

        // Constructor (tùy chọn: dùng để inject logger)
        public ServerStatusController(ILogger<ServerStatusController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Hàm kiểm tra trạng thái server.
        /// </summary>
        /// <returns>Trả về một thông báo JSON "Server is running!".</returns>
        [HttpGet("status")] // Định nghĩa đây là một HTTP GET request, route là /api/testserver/status
        public IActionResult GetServerStatus()
        {
            _logger.LogInformation("GET request to /api/testserver/status received.");
            return Ok(new { message = "Server is running!", timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// Hàm trả về thông báo tùy chỉnh dựa trên đầu vào.
        /// </summary>
        /// <param name="name">Tên bạn muốn gửi lời chào.</param>
        /// <returns>Trả về một thông báo chào mừng.</returns>
        [HttpGet("status/{name}")] // Định nghĩa HTTP GET request với tham số trong route: /api/testserver/hello/John
        public IActionResult SayHello(string name)
        {
            _logger.LogInformation($"GET request to /api/testserver/hello/{name} received.");
            return Ok($"Hello, {name}! Your backend is working.");
        }

    }
}