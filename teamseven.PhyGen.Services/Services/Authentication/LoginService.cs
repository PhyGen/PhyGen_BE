using Microsoft.AspNetCore.Identity.Data;
using teamseven.PhyGen.Repository.Repository;
using teamseven.PhyGen.Services.Interfaces;
using TeamSeven.PhyGen.Services.Object.Requests;

namespace teamseven.PhyGen.Services.Services.Authentication
{
    public class LoginService : ILoginService
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordEncryptionService _passwordEncryptionService;
        private readonly IAuthService _authService;

        public LoginService(
            UserRepository userRepository,
            IPasswordEncryptionService passwordEncryptionService,
            IAuthService authService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordEncryptionService = passwordEncryptionService ?? throw new ArgumentNullException(nameof(passwordEncryptionService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public async Task<(bool IsSuccess, string ResultOrError)> ValidateUserAsync(TeamSeven.PhyGen.Services.Object.Requests.LoginRequest loginRequest)
        {
            // Lấy user từ repository
            var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                return (false, "Wrong email or password");
            }

            // So sánh mật khẩu hash
            if (!_passwordEncryptionService.VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                return (false, "Invalid password");
            }

            // Kiểm tra tài khoản có bị vô hiệu hóa không
            if (!user.IsActive.GetValueOrDefault())
            {
                return (false, "This account is disabled");
            }


            //set last login
            await _userRepository.SetLoginDateTime(user.Id);


            // Tạo và trả về token
            var token = _authService.GenerateJwtToken(user);

            return (true, token);
        }


        //private void ValidateInput(string email, string password)
        //{
        //    if (string.IsNullOrEmpty(email))
        //        throw new ArgumentException("Email is required.", nameof(email));
        //    if (string.IsNullOrEmpty(password))
        //        throw new ArgumentException("Password is required.", nameof(password));
        //    if (!IsValidEmail(email))
        //        throw new ArgumentException("Invalid email format.", nameof(email));
        //}

        //private async Task<User> ValidateUserExistence(string email)
        //{
        //    User user = await _userRepository.GetByEmailAsync(email);
        //    if (user == null)
        //        throw new KeyNotFoundException("User not found.");
        //    return user;
        //}

        //private void ValidatePassword(string password, string hashedPassword)
        //{
        //    if (!_passwordEncryptionService.VerifyPassword(password, hashedPassword))
        //        throw new UnauthorizedAccessException("Invalid password.");
        //}

        //private bool IsValidEmail(string email)
        //{
        //    // Regex đơn giản để kiểm tra định dạng email
        //    string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        //    return Regex.IsMatch(email, pattern);
        //}
    }
}
