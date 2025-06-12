using Microsoft.AspNetCore.Mvc;
using teamseven.PhyGen.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using TeamSeven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        /// <summary>
        /// Authenticate user and return JWT token.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(
            Summary = "User login",
            Description = "Authenticates the user and returns a JWT token."
        )]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (isSuccess, resultOrError) = await _loginService.ValidateUserAsync(request);
            if (!isSuccess)
            {
                return Unauthorized(new { message = resultOrError });
            }

            return Ok(new { token = resultOrError, message = "Login successful" });
        }
    }
}