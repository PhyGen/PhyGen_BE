using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Services.ServiceProvider;

namespace teamseven.PhyGen.Controllers
{
    [ApiController]
    [Route("api/questions")]
    [Produces("application/json")]
    public class QuestionsController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(IServiceProviders serviceProvider, ILogger<QuestionsController> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a new question.
        /// </summary>
       
        [HttpPost]
        [Authorize(Policy = "DeliveringStaffPolicy")]
        [SwaggerOperation(Summary = "Create a new question", Description = "Creates a new question with the provided details.")]
        [SwaggerResponse(201, "Question created successfully.", typeof(QuestionDataResponse))]
        [SwaggerResponse(400, "Invalid request data, e.g., missing required fields or invalid difficulty level.", typeof(ProblemDetails))]
        [SwaggerResponse(404, "Lesson or user not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDataRequest questionDataRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for QuestionDataRequest.");
                return BadRequest(ModelState);
            }

            try
            {
                await _serviceProvider.QuestionsService.AddQuestionAsync(questionDataRequest);
                _logger.LogInformation("Question created");

                return Ok("Created quesition successfully");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found error: {Message}", ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question: {Message}", ex.Message);
                return StatusCode(500, new { Message = "An error occurred while creating the question." });
            }
        }

        /// <summary>
        /// Deletes a question by ID.
        /// </summary>
        /// <remarks>
        /// Deletes the question with the specified ID if it exists.
        /// 
        /// Sample request:
        /// 
        ///     DELETE /api/questions/1
        /// 
        /// Sample response:
        /// 
        ///     No content (204)
        /// </remarks>
        /// <param name="id">The ID of the question to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a question", Description = "Deletes a question by its ID.")]
        [SwaggerResponse(204, "Question deleted successfully.")]
        [SwaggerResponse(404, "Question not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                await _serviceProvider.QuestionsService.DeleteQuestionAsync(id);
                _logger.LogInformation("Question with ID {QuestionId} deleted via API.", id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found error: {Message}", ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question with ID {QuestionId}: {Message}", id, ex.Message);
                return StatusCode(500, new { Message = "An error occurred while deleting the question." });
            }
        }

        /// <summary>
        /// Gets a question by ID (placeholder for CreatedAtAction).
        /// </summary>
        /// <param name="id">The question ID.</param>
        /// <returns>The question details.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a question by ID", Description = "Retrieves a question by its ID (placeholder).")]
        [SwaggerResponse(200, "Question found.", typeof(QuestionDataResponse))]
        [SwaggerResponse(404, "Question not found.", typeof(ProblemDetails))]
        public IActionResult GetQuestion(int id)
        {
            // Placeholder: Implement actual logic in a real scenario
            return Ok(new { Id = id });
        }
    }
}