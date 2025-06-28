using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Services.ServiceProvider;

namespace teamseven.PhyGen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ChapterController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly ILogger<ChapterController> _logger;

        public ChapterController(IServiceProviders serviceProvider, ILogger<ChapterController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new chapter", Description = "Creates a new chapter with the provided details.")]
        [SwaggerResponse(201, "Chapter created successfully.")]
        [SwaggerResponse(400, "Invalid request data.", typeof(ProblemDetails))]
        [SwaggerResponse(404, "Semester not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> CreateChapter([FromBody] CreateChapterRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CreateChapterRequest.");
                return BadRequest(ModelState);
            }

            try
            {
                await _serviceProvider.ChapterService.CreateChapterAsync(request);
                return StatusCode(201, new { Message = "Chapter created successfully." });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new { Message = "An error occurred while creating the chapter." });
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a chapter", Description = "Deletes a chapter by its ID.")]
        [SwaggerResponse(204, "Chapter deleted successfully.")]
        [SwaggerResponse(404, "Chapter not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteChapter(int id)
        {
            try
            {
                await _serviceProvider.ChapterService.DeleteChapterAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new { Message = "An error occurred while deleting the chapter." });
            }
        }
    }
}
