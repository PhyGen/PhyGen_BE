using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using teamseven.PhyGen.Services.Services.ServiceProvider;

namespace teamseven.PhyGen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;

        public AccountController(IServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        [HttpGet]
        [Authorize(Policy = "DeliveringStaffPolicy")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _serviceProvider.UserService.GetUsersAsync());

        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID");
            }
            try
            {
                var user = await _serviceProvider.UserService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the user");
            }
        }

    }
}