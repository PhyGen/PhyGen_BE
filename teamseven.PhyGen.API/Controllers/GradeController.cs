using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services.Services.ServiceProvider;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;
using Microsoft.AspNetCore.Authorization;
using teamseven.PhyGen.Services.Object.Responses;

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

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all grades", Description = "Retrieves all grades.")]
        [SwaggerResponse(200, "Grades retrieved successfully.", typeof(IEnumerable<GradeDataResponse>))]
        public async Task<IActionResult> GetAllGrades()
        {
            var grades = await _serviceProvider.GradeService.GetAllGradesAsync();
            return Ok(grades);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get grade by ID", Description = "Retrieves a grade by its ID.")]
        [SwaggerResponse(200, "Grade found.", typeof(GradeDataResponse))]
        [SwaggerResponse(404, "Grade not found.")]
        public async Task<IActionResult> GetGradeById(int id)
        {
            try
            {
                var grade = await _serviceProvider.GradeService.GetGradeByIdAsync(id);
                return Ok(grade);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { Message = ex.Message });
            }
        }


        /// <summary>
        /// Creates a new grade.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "SaleStaffPolicy")]
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

        [HttpPut("{id}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        [SwaggerOperation(Summary = "Update a grade", Description = "Updates a grade by ID.")]
        [SwaggerResponse(200, "Grade updated successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        [SwaggerResponse(404, "Grade not found.")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] GradeDataRequest request)
        {
            if (!ModelState.IsValid || id != request.Id)
            {
                _logger.LogWarning("Invalid update request.");
                return BadRequest(new { Message = "Invalid data or ID mismatch." });
            }

            try
            {
                await _serviceProvider.GradeService.UpdateGradeAsync(request);
                return Ok(new { Message = "Grade updated successfully." });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating grade.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }


        /// <summary>
        /// Deletes a grade by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "SaleStaffPolicy")]
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
