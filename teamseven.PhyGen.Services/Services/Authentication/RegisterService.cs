using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Repository.Repository;
using teamseven.PhyGen.Services.Interfaces;

namespace teamseven.PhyGen.Services.Services.Authentication
{
    public class RegisterService : IRegisterService
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordEncryptionService _passwordEncryptionService;
        private readonly IEmailService _emailService;
        private readonly ImageRepository _imageRepository;
        private readonly IConfiguration _configuration;

        public RegisterService(
            UserRepository userRepository,
            IPasswordEncryptionService passwordEncryptionService,
            IEmailService emailService, ImageRepository imageRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordEncryptionService = passwordEncryptionService ?? throw new ArgumentNullException(nameof(passwordEncryptionService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task RegisterUserAsync(string email, string password, string name)
        {
            // Validation
            ValidateInput(email, password, name);
            await ValidateEmailUniqueness(email);

            ////get user img
            //await _imageRepository.AddImageAsync(new Image(img, null));

            // Tạo user mới
            var user = new User
            {
                Email = email,
                EncryptedPassword = _passwordEncryptionService.EncryptPassword(password),
                FullName = name,
                CreatedAt = DateTime.Now,
                Role = "User",
                //ImageId = await _imageRepository.AddImageAndReturnPK(new Image (img,null)),
                ImageId = 0
            };

            // Lưu vào database và gửi email
            await _userRepository.AddUserAsync(user);
            SendWelcomeMail(user);
        }

        private void ValidateInput(string email, string password, string name)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is required.", nameof(email));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required.", nameof(password));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name is required.", nameof(name));
        }

        private async Task ValidateEmailUniqueness(string email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already in use.");
        }

        private void SendWelcomeMail(User user)
        {
            string subject = "Welcome to Musik";
            string body = $"Hello {user.Email},\n\nWelcome to Musik - A free music web player";
            _emailService.SendEmail(user.Email, subject, body);
        }

        public async Task<int> ChangeUserRole(int userId, string roleName, string providedSecretKey)
        {
            if (userId < 0)
                throw new ArgumentException(nameof(userId), "User ID cannot be negative.");

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentException(nameof(roleName), "Role name cannot be empty.");

            // Retrieve secret key from configuration or environment variable
            var expectedSecretKey = _configuration["Security:SuperSecretKey"]
                                    ?? Environment.GetEnvironmentVariable("SUPER_SECRET_KEY");

            if (string.IsNullOrEmpty(expectedSecretKey) || providedSecretKey != expectedSecretKey)
                throw new UnauthorizedAccessException("CÚT");

           return await _userRepository.ChangeUserRoleAsync(userId, roleName);
        }

    }
}
