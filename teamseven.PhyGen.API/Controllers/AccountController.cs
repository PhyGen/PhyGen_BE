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

    }
}