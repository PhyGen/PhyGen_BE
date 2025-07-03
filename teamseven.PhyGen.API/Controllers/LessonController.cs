using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Services.ServiceProvider;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LessonController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly ILogger<LessonController> _logger;

        public LessonController(IServiceProviders serviceProvider, ILogger<LessonController> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all lessons", Description = "Retrieves a list of all lessons.")]
        [SwaggerResponse(200, "List of lessons returned successfully.", typeof(IEnumerable<LessonDataResponse>))]
        public async Task<IActionResult> GetAllLessons()
        {
            var result = await _serviceProvider.LessonService.GetAllLessonAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get lesson by ID", Description = "Retrieves a lesson by its ID.")]
        [SwaggerResponse(200, "Lesson found.", typeof(LessonDataResponse))]
        [SwaggerResponse(404, "Lesson not found.", typeof(ProblemDetails))]
        public async Task<IActionResult> GetLessonById(int id)
        {
            try
            {
                var result = await _serviceProvider.LessonService.GetLessonByIdAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new lesson.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "SaleStaffPolicy")]
        [SwaggerOperation(Summary = "Create a new lesson", Description = "Creates a new lesson under the specified chapter.")]
        [SwaggerResponse(201, "Lesson created successfully.")]
        [SwaggerResponse(400, "Invalid request data.", typeof(ProblemDetails))]
        [SwaggerResponse(404, "Chapter not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> CreateLesson([FromBody] CreateLessonRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CreateLessonRequest.");
                return BadRequest(ModelState);
            }

            try
            {
                await _serviceProvider.LessonService.CreateLessonAsync(request);
                _logger.LogInformation("Lesson created successfully.");
                return StatusCode(201, new { Message = "Lesson created successfully." });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Chapter not found: {Message}", ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating lesson.");
                return StatusCode(500, new { Message = "An error occurred while creating the lesson." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        [SwaggerOperation(Summary = "Update a lesson", Description = "Updates a lesson by ID.")]
        [SwaggerResponse(200, "Lesson updated successfully.")]
        [SwaggerResponse(400, "Invalid input.", typeof(ProblemDetails))]
        [SwaggerResponse(404, "Lesson not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] LessonDataRequest request)
        {
            if (!ModelState.IsValid || id != request.Id)
                return BadRequest(ModelState);

            try
            {
                await _serviceProvider.LessonService.UpdateLessonAsync(request);
                return Ok(new { Message = "Lesson updated successfully." });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating lesson.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        /// <summary>
        /// Deletes a lesson by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        [SwaggerOperation(Summary = "Delete a lesson", Description = "Deletes a lesson by its ID.")]
        [SwaggerResponse(204, "Lesson deleted successfully.")]
        [SwaggerResponse(404, "Lesson not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            try
            {
                await _serviceProvider.LessonService.DeleteLessonAsync(id);
                _logger.LogInformation("Lesson with ID {Id} deleted.", id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson not found: {Message}", ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lesson with ID {Id}: {Message}", id, ex.Message);
                return StatusCode(500, new { Message = "An error occurred while deleting the lesson." });
            }
        }
    }
}
