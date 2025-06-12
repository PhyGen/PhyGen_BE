using Microsoft.AspNetCore.Mvc;
using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Object.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace teamseven.PhyGen.API.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        private readonly IAuthService _authService;
        private readonly ILoginService _loginService;

        public RegisterController(
            IRegisterService registerService,
            IAuthService authService,
            ILoginService loginService)
        {
            _registerService = registerService ?? throw new ArgumentNullException(nameof(registerService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        /// <summary>
        /// Register a new user and return JWT token.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(
            Summary = "User registration",
            Description = "Registers a new user, authenticates, and returns a JWT token."
        )]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
        {
            // Validation cơ bản bởi Data Annotations và [ApiController]
            var (isRegisterSuccess, registerError) = await _registerService.RegisterUserAsync(request);
            if (!isRegisterSuccess)
            {
                return BadRequest(new { message = registerError });
            }

            return Ok(new {message = "Registration successful. Please login again." });
        }

        /// <summary>
        /// Change user role.
        /// </summary>
        [HttpPost("change-role")]
        [SwaggerOperation(
            Summary = "Change user role",
            Description = "Changes the role of a user if the correct secret key is provided."
        )]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleRequest request)
        {
            // Validation cơ bản bởi Data Annotations và [ApiController]
            var (isSuccess, resultOrError) = await _registerService.ChangeUserRoleAsync(request.UserId, request.RoleName, request.SecretKey);
            if (!isSuccess)
            {
                return BadRequest(new { message = resultOrError });
            }

            return Ok(new { message = resultOrError });
        }
    }
}