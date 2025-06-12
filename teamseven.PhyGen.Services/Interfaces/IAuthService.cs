using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        //Task<(bool IsSuccess, string AccessToken, string ErrorMessage)> RefreshAccessTokenAsync(string refreshToken);
        bool IsUserInRole(string authHeader, string role);
        bool IsUserInPlan(string token, string plan);
    }
}