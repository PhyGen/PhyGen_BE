using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Services.ServiceProvider;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Controllers
{
    [ApiController]
    [Route("api/exams")]
    [Produces("application/json")]
    public class ExamController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly ILogger<ExamController> _logger;

        public ExamController(IServiceProviders serviceProvider, ILogger<ExamController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        // =================== GET ALL EXAMS ===================

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all exams", Description = "Retrieves all exams")]
        public async Task<IActionResult> GetAllExams()
        {
            var exams = await _serviceProvider.ExamService.GetAllExamAsync();
            return Ok(exams);
        }

        // =================== GET EXAM BY ID ===================

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get exam by ID", Description = "Retrieves a single exam")]
        public async Task<IActionResult> GetExam(int id)
        {
            try
            {
                var exam = await _serviceProvider.ExamService.GetExamAsync(id);
                return Ok(exam);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { Message = ex.Message });
            }
        }

        // =================== CREATE EXAM ===================

        [HttpPost]
        [Authorize(Policy = "SaleStaffPolicy")]
        [SwaggerOperation(Summary = "Create exam", Description = "Creates a new exam")]
        public async Task<IActionResult> CreateExam([FromBody] ExamRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _serviceProvider.ExamService.CreateExamAsync(request);
                return StatusCode(201, new { Message = "Exam created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exam");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        // =================== ADD QUESTION TO EXAM ===================

        [HttpPost("questions")]
        [Authorize(Policy = "DeliveringStaffPolicy")]
        [SwaggerOperation(Summary = "Assign question to exam")]
        public async Task<IActionResult> AddExamQuestion([FromBody] ExamQuestionRequest request)
        {
            try
            {
                await _serviceProvider.ExamService.CreateExamQuestionAsync(request);
                return Ok(new { Message = "Question added to exam." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning question");
                return BadRequest(new { Message = ex.Message });
            }
        }

        // =================== REMOVE QUESTION FROM EXAM ===================

        [HttpDelete("questions")]
        [Authorize(Policy = "DeliveringStaffPolicy")]
        [SwaggerOperation(Summary = "Remove question from exam")]
        public async Task<IActionResult> RemoveExamQuestion([FromBody] ExamQuestionRequest request)
        {
            try
            {
                await _serviceProvider.ExamService.RemoveExamQuestion(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { Message = ex.Message });
            }
        }

        // =================== GET QUESTIONS BY EXAM ID ===================

        [HttpGet("{id}/questions")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get exam questions by ExamId")]
        public async Task<IActionResult> GetExamQuestions(int id)
        {
            var questions = await _serviceProvider.ExamService.GetExamQuestionByIdAsync(id);
            return Ok(questions);
        }

        // =================== CREATE EXAM HISTORY ===================

        [HttpPost("history")]
        [Authorize]
        [SwaggerOperation(Summary = "Create exam history")]
        public async Task<IActionResult> CreateExamHistory([FromBody] ExamHistoryRequest request)
        {
            try
            {
                await _serviceProvider.ExamService.CreateExamHistoryAsync(request);
                return StatusCode(201, new { Message = "Exam history recorded." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exam history");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        // =================== DELETE EXAM HISTORY ===================

        [HttpDelete("history")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete exam history")]
        public async Task<IActionResult> DeleteExamHistory([FromBody] ExamHistoryRequest request)
        {
            try
            {
                await _serviceProvider.ExamService.DeleteExamHistoryAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exam history");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        // =================== GET EXAM HISTORY DETAILS ===================

        [HttpGet("history/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get exam history details")]
        public async Task<IActionResult> GetExamHistory(int id)
        {
            try
            {
                var result = await _serviceProvider.ExamService.GetExamHistoryResponseAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exam history");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }
}
