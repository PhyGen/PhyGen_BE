using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services.Services.ServiceProvider;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GradeController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly ILogger<GradeController> _logger;

        public GradeController(IServiceProviders serviceProvider, ILogger<GradeController> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a new grade.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new grade", Description = "Creates a new grade with the provided details.")]
        [SwaggerResponse(201, "Grade created successfully.")]
        [SwaggerResponse(400, "Invalid request data.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> CreateGrade([FromBody] CreateGradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CreateGradeRequest.");
                return BadRequest(ModelState);
            }

            try
            {
                await _serviceProvider.GradeService.CreateGradeAsync(request);
                _logger.LogInformation("Grade created successfully.");
                return StatusCode(201, new { Message = "Grade created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating grade.");
                return StatusCode(500, new { Message = "An error occurred while creating the grade." });
            }
        }

        /// <summary>
        /// Deletes a grade by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a grade", Description = "Deletes a grade by its ID.")]
        [SwaggerResponse(204, "Grade deleted successfully.")]
        [SwaggerResponse(404, "Grade not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            try
            {
                await _serviceProvider.GradeService.DeleteGradeAsync(id);
                _logger.LogInformation("Grade with ID {Id} deleted.", id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Grade not found: {Message}", ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting grade with ID {Id}: {Message}", id, ex.Message);
                return StatusCode(500, new { Message = "An error occurred while deleting the grade." });
            }
        }
    }
}
