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
    public class SemesterController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly ILogger<SemesterController> _logger;

        public SemesterController(IServiceProviders serviceProvider, ILogger<SemesterController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new semester", Description = "Creates a new semester with the provided details.")]
        [SwaggerResponse(201, "Semester created successfully.")]
        [SwaggerResponse(400, "Invalid request data.", typeof(ProblemDetails))]
        [SwaggerResponse(404, "Grade not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CreateSemesterRequest.");
                return BadRequest(ModelState);
            }

            try
            {
                await _serviceProvider.SemesterService.CreateSemesterAsync(request);
                return StatusCode(201, new { Message = "Semester created successfully." });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new { Message = "An error occurred while creating the semester." });
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a semester", Description = "Deletes a semester by its ID.")]
        [SwaggerResponse(204, "Semester deleted successfully.")]
        [SwaggerResponse(404, "Semester not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteSemester(int id)
        {
            try
            {
                await _serviceProvider.SemesterService.DeleteSemesterAsync(id);
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
                return StatusCode(500, new { Message = "An error occurred while deleting the semester." });
            }
        }
    }
}
