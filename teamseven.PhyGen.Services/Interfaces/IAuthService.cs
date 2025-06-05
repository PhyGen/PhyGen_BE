using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Models;


namespace teamseven.PhyGen.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Kiểm tra xem người dùng có vai trò (role) cụ thể hay không dựa trên token trong header.
        /// </summary>
        /// <param name="authHeader">Header Authorization chứa token (Bearer).</param>
        /// <param name="role">Vai trò cần kiểm tra.</param>
        /// <returns>True nếu người dùng có vai trò, ngược lại False.</returns>
        bool IsUserInRole(string authHeader, string role);

        /// <summary>
        /// Kiểm tra xem người dùng có thuộc loại tài khoản (plan) cụ thể hay không dựa trên token.
        /// </summary>
        /// <param name="token">Token JWT.</param>
        /// <param name="plan">Loại tài khoản cần kiểm tra.</param>
        /// <returns>True nếu người dùng thuộc loại tài khoản, ngược lại False.</returns>
        bool IsUserInPlan(string token, string plan);

        /// <summary>
        /// Tạo token JWT cho người dùng dựa trên thông tin của họ.
        /// </summary>
        /// <param name="user">Thông tin người dùng.</param>
        /// <returns>Chuỗi token JWT.</returns>
        string GenerateJwtToken(User? user);

    }
}
