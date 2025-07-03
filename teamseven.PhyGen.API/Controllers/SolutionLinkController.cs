using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services.Services.SolutionLinkService;

namespace teamseven.PhyGen.Api.Controllers
{
    [ApiController]
    [Route("api/solution-links")]
    public class SolutionLinkController : ControllerBase
    {
        private readonly ISolutionLinkService _service;
        private readonly ILogger<SolutionLinkController> _logger;

        public SolutionLinkController(ISolutionLinkService service, ILogger<SolutionLinkController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SolutionLinkRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _service.AddAsync(request);
            return Ok(new { message = "Link created successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("solution/{solutionId}")]
        public async Task<IActionResult> GetBySolution(int solutionId)
        {
            var result = await _service.GetBySolutionIdAsync(solutionId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SolutionLinkRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _service.UpdateAsync(id, request);
            return Ok(new { message = "Link updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Link deleted successfully." });
        }
    }
}
