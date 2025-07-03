using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;
using teamseven.PhyGen.Services.Services.ServiceProvider;

namespace teamseven.PhyGen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserSubscriptionController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly ILogger<UserSubscriptionController> _logger;

        public UserSubscriptionController(IServiceProviders serviceProvider, ILogger<UserSubscriptionController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all user subscriptions")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceProvider.UserSubscriptionService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get a user subscription by ID")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _serviceProvider.UserSubscriptionService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeliveringStaffPolicy")]
        [SwaggerOperation(Summary = "Create a new user subscription")]
        public async Task<IActionResult> Create([FromBody] CreateUserSubscriptionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _serviceProvider.UserSubscriptionService.CreateAsync(request);
            return StatusCode(201, new { Message = "User subscription created successfully." });
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "DeliveringStaffPolicy")]
        [SwaggerOperation(Summary = "Update an existing user subscription")]
        public async Task<IActionResult> Update(int id, [FromBody] UserSubscriptionDataRequest request)
        {
            if (!ModelState.IsValid || id != request.Id)
                return BadRequest(new { Message = "Invalid data or ID mismatch." });

            try
            {
                await _serviceProvider.UserSubscriptionService.UpdateAsync(request);
                return Ok(new { Message = "User subscription updated successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeliveringStaffPolicy")]
        [SwaggerOperation(Summary = "Delete a user subscription")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _serviceProvider.UserSubscriptionService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
