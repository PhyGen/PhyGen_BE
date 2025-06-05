using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Interfaces;

namespace teamseven.PhyGen.Services.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
        new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Email
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID
        new Claim("userid", user.UserId.ToString()), // User ID
        new Claim("fullname", user.FullName), // Full Name
        new Claim("email", user.Email), // Email
        new Claim("img", user.ImageId?.ToString() ?? "null"), // Image 
        new Claim("role", user.Role ?? "User") // Role (Admin, User...)
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

    }
}
