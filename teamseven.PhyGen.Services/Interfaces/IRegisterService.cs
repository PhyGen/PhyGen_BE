using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Services.Interfaces
{
    public interface IRegisterService
    {
        /// <summary>
        /// Đăng ký người dùng mới với email, mật khẩu, tên và ảnh đại diện.
        /// </summary>
        /// <param name="email">Email người dùng.</param>
        /// <param name="password">Mật khẩu người dùng.</param>
        /// <param name="name">Tên người dùng.</param>
        /// <param name="img">Link ảnh đại diện (có thể null).</param>
        /// <returns>Tuple chứa kết quả thành công (bool) và thông báo (string).</returns>
        Task RegisterUserAsync(string email, string password, string name);

        Task<int> ChangeUserRole(int userId, string roleName, string superSecretKey);

    }
}
