using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Interfaces;

namespace teamseven.PhyGen.Services.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public AuthService()
        {
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (jsonToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var identity = new ClaimsIdentity(jsonToken?.Claims, "jwt");
            return new ClaimsPrincipal(identity);
        }
        public bool IsUserInRole(string authHeader, string role)
        {
            try
            {
                var tokenString = authHeader.Substring("Bearer ".Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(tokenString);
                var roleClaim = token.Claims.FirstOrDefault(c => c.Type == "role")?.Value?.ToLower();

                return roleClaim == role.ToLower();
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public IActionResult ValidateAuthorizationHeader(Microsoft.AspNetCore.Http.IHeaderDictionary headers)
        //{
        //    if (!headers.TryGetValue("Authorization", out var authHeader) ||
        //        string.IsNullOrWhiteSpace(authHeader) ||
        //        !authHeader.ToString().StartsWith("Bearer "))
        //    {
        //        return new UnauthorizedObjectResult(new { message = "Missing or invalid Authorization header." });
        //    }

        //    if (!IsUserInRole(authHeader, "admin"))
        //    {
        //        return new ForbidResult();
        //    }

        //    return null;
        //}
        //public bool IsUserInRole(string token, string role)
        //{
        //    try
        //    {
        //        var principal = GetPrincipalFromExpiredToken(token);
        //        var roleClaim = principal?.FindFirst(ClaimTypes.Role);
        //        if (roleClaim != null && roleClaim.Value == role)
        //        {
        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public bool IsUserInPlan(string token, string plan)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                var planClaim = principal?.FindFirst("AccountType"); // Lấy claim 

                if (planClaim != null && planClaim.Value == plan)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string GenerateJwtToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null when generating token.");

            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing in configuration.");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim("fullname", user.FullName ?? string.Empty),
                new Claim("email", user.Email),
                new Claim("roleId", user.RoleId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(120), // Expiration reduced to a reasonable 2 hour
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> GoogleLoginAsync(string idToken)
        {
            try
            {
                // Xác thực token với Google
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                // Kiểm tra UserSocialProvider
                var socialProvider = await _unitOfWork.UserSocialProviderRepository.GetByProviderAsync("Google", payload.Subject);
                User user;

                if (socialProvider == null)
                {
                    // Kiểm tra người dùng bằng email
                    user = await _unitOfWork.UserRepository.GetByEmailAsync(payload.Email);
                    if (user == null)
                    {
                        // Tạo người dùng mới
                        user = new User
                        {
                            Email = payload.Email,
                            FullName = payload.Name,
                            AvatarUrl = payload.Picture,
                            RoleId = 1, // Giả sử RoleId mặc định
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _unitOfWork.UserRepository.AddUserAsync(user);
                    }

                    // Tạo UserSocialProvider
                    var userSocialProvider = new UserSocialProvider
                    {
                        UserId = user.Id,
                        ProviderName = "Google",
                        ProviderId = payload.Subject,
                        Email = payload.Email,
                        ProfileUrl = payload.Picture,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.UserSocialProviderRepository.AddAsync(userSocialProvider);

                    // Lưu thay đổi với giao dịch
                    await _unitOfWork.SaveChangesWithTransactionAsync();
                }
                else
                {
                    // Người dùng hiện có
                    user = await _unitOfWork.UserRepository.GetByEmailAsync(payload.Email);
                    if (user == null)
                    {
                        throw new Exception("User not found.");
                    }
                    user.LastLoginAt = DateTime.UtcNow;
                    await _unitOfWork.UserRepository.UpdateAsync(user);
                    await _unitOfWork.SaveChangesWithTransactionAsync();
                }

                // Tạo JWT
                return GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xác thực Google.", ex);
            }
        }
    }
}
