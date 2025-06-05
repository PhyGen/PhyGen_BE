using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Object.Requests;

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

        [HttpPost]
        [SwaggerOperation(
            Summary = "đăng kí",
            Description = "đăng kí tài khoản"
        )]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request body cannot be null." });
            }

            try
            {
                await _registerService.RegisterUserAsync(request.Email, request.Password, request.FullName);

                var user = await _loginService.ValidateUserAsync(request.Email, request.Password);
                if (user == null)
                {
                    return StatusCode(500, new { message = "Failed to retrieve user after registration." });
                }

                //// Tạo token JWT
                //string token = _authService.GenerateJwtToken(user);
                return Ok("Registration successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {

                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khác (database, email service, v.v.)
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }
        [HttpPost("/change-role")]
        public async Task<IActionResult> ChangeRole(int userId, string roleName, string supersecretkey)
        {
            try
            {
                await _registerService.ChangeUserRole(userId, roleName, supersecretkey);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }

}
