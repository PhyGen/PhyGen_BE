using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Services;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;
using teamseven.PhyGen.Services.Services.ServiceProvider;

namespace teamseven.PhyGen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Get questions",
            Description = "Retrieves a list of questions. Use pageNumber and pageSize (default 10) query parameters for pagination, or omit them to get all questions."
        )]
        [SwaggerResponse(200, "Questions retrieved successfully.", typeof(PagedResponse<QuestionDataResponse>))]
        [SwaggerResponse(400, "Invalid pagination parameters.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestions([FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            try
            {
                if (pageNumber.HasValue && pageNumber < 1 || pageSize.HasValue && pageSize < 1)
                {
                    _logger.LogWarning("Invalid pagination parameters: pageNumber={PageNumber}, pageSize={PageSize}.", pageNumber, pageSize);
                    return BadRequest(new { Message = "pageNumber and pageSize must be greater than 0." });
                }

                var pagedQuestions = await _serviceProvider.QuestionsService.GetQuestionsAsync(pageNumber, pageSize);
                _logger.LogInformation("Retrieved {Count} questions for page {PageNumber}.", pagedQuestions.Items.Count, pagedQuestions.PageNumber);
                return Ok(pagedQuestions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving questions: {Message}", ex.Message);
                return StatusCode(500, new { Message = "An error occurred while retrieving questions." });
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeliveringStaffPolicy")]
        [SwaggerOperation(Summary = "Create a new question", Description = "Creates a new question with the provided details.")]
        [SwaggerResponse(201, "Question created successfully.", typeof(QuestionDataResponse))]
        [SwaggerResponse(400, "Invalid request data.", typeof(ProblemDetails))]
        [SwaggerResponse(404, "Lesson or user not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDataRequest questionDataRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request data for creating question.");
                return BadRequest(ModelState);
            }

            try
            {
                await _serviceProvider.QuestionsService.AddQuestionAsync(questionDataRequest);
                _logger.LogInformation("Question created successfully.");
                return StatusCode(201, new { Message = "Question created successfully." });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found: {Message}", ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question: {Message}", ex.Message);
                return StatusCode(500, new { Message = "An error occurred while creating question." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeliveringStaffPolicy")]
        [SwaggerOperation(Summary = "Delete a question", Description = "Deletes a question by its ID.")]
        [SwaggerResponse(204, "Question deleted successfully.")]
        [SwaggerResponse(404, "Question not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                await _serviceProvider.QuestionsService.DeleteQuestionAsync(id);
                _logger.LogInformation("Question with ID {QuestionId} deleted successfully.", id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found: {Message}", ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question with ID {QuestionId}: {Message}", id, ex.Message);
                return StatusCode(500, new { Message = "An error occurred while deleting question." });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get question by ID", Description = "Retrieves a question by its ID.")]
        [SwaggerResponse(200, "Question retrieved successfully.", typeof(QuestionDataResponse))]
        [SwaggerResponse(404, "Question not found.", typeof(ProblemDetails))]
        [SwaggerResponse(500, "Internal server error.", typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestion(int id)
        {
            try
            {
                ////var question = await _serviceProvider.QuestionsService.GetQuestionByIdAsync(id);
                //_logger.LogInformation("Question with ID {QuestionId} retrieved successfully.", id);
                return Ok("Not implemented yet. Dial tri admin.");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found: {Message}", ex.Message);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving question with ID {QuestionId}: {Message}", id, ex.Message);
                return StatusCode(500, new { Message = "An error occurred while retrieving question." });
            }
        }
    }
}