using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Object;
using Swashbuckle.AspNetCore.Annotations;

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
        public async Task<IActionResult> Login([FromBody] teamseven.PhyGen.Services.Object.Requests.LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request body cannot be null." });
            }

            try
            {
                string token = await _loginService.ValidateUserAsync(request.Email, request.Password);
                return Ok(new { token, message = "Login successful." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found." });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid password." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }
    }
}