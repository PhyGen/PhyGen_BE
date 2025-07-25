using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Services.ServiceProvider;
using teamseven.PhyGen.Services.Extensions;
using Microsoft.AspNetCore.Authorization;
using teamseven.PhyGen.Services.Object.Responses;


namespace teamseven.PhyGen.Controllers
{
    [ApiController]
    [Route("api/solutions")]
    [Produces("application/json")]
    public class SolutionController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly ILogger<SolutionController> _logger;

        public SolutionController(IServiceProviders serviceProvider, ILogger<SolutionController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all solutions", Description = "Retrieves all solutions.")]
        [SwaggerResponse(200, "Solutions retrieved successfully.", typeof(IEnumerable<SolutionDataResponse>))]
        public async Task<IActionResult> GetAll()
        {
            var data = await _serviceProvider.SolutionService.GetAllSolutionsAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get solution by ID", Description = "Retrieves a solution by ID.")]
        [SwaggerResponse(200, "Solution found", typeof(SolutionDataResponse))]
        [SwaggerResponse(404, "Solution not found")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _serviceProvider.SolutionService.GetSolutionByIdAsync(id);
                return Ok(data);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Create a new solution", Description = "Creates a new solution without video.")]
        [SwaggerResponse(201, "Solution created successfully")]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> Create([FromBody] CreateSolutionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _serviceProvider.SolutionService.CreateSolutionAsync(request);
            return StatusCode(201, new { message = "Solution created successfully." });
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        [SwaggerOperation(Summary = "Update a solution", Description = "Updates a solution.")]
        [SwaggerResponse(200, "Solution updated successfully")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Solution not found")]
        public async Task<IActionResult> Update(int id, [FromBody] SolutionDataRequest request)
        {
            if (!ModelState.IsValid || request.Id != id)
                return BadRequest(new { message = "Invalid request data." });

            try
            {
                await _serviceProvider.SolutionService.UpdateSolutionAsync(request);
                return Ok(new { message = "Solution updated successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        [SwaggerOperation(Summary = "Delete a solution", Description = "Deletes a solution.")]
        [SwaggerResponse(204, "Deleted successfully")]
        [SwaggerResponse(404, "Solution not found")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _serviceProvider.SolutionService.DeleteSolutionAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("video")]
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Create a new solution with video", Description = "Uploads video and creates a solution with video URL.")]
        [SwaggerResponse(200, "Solution with video created successfully")]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> AddSolutionWithVideo([FromForm] SolutionWithVideoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _serviceProvider.SolutionService.AddSolutionWithVideoAsync(request);
                return Ok(new { message = "Solution with video created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create solution with video.");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/video")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get video URL by solution ID", Description = "Returns the video URL associated with the solution.")]
        [SwaggerResponse(200, "Video URL found")]
        [SwaggerResponse(404, "Video not found")]
        public async Task<IActionResult> GetSolutionVideo(int id)
        {
            try
            {
                var solution = await _serviceProvider.SolutionService.GetSolutionByIdAsync(id);

                if (string.IsNullOrWhiteSpace(solution.VideoData))
                {
                    return NotFound(new { message = "Video URL not found for this solution." });
                }

                return Ok(new { url = solution.VideoData });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPut("{id}/video")]
        //[Authorize(Policy = "SaleStaffPolicy")] 
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "Update solution video",
            Description = "Uploads new video and updates the existing solution with video URL and metadata."
        )]
        [SwaggerResponse(200, "Solution video updated successfully")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Solution not found")]
        public async Task<IActionResult> UpdateSolutionVideo(int id, [FromForm] SolutionWithVideoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                request.SolutionId = id;
                await _serviceProvider.SolutionService.UpdateSolutionVideoAsync(request);
                return Ok(new { message = "Solution video updated successfully." });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update solution video.");
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
