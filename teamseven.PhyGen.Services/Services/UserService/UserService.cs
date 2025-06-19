using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.UserService
{
    public interface IUserService
    {
        Task <IEnumerable<User>> GetUsersAsync ();
        Task <UserResponse> GetUserByIdAsync (int id);
        Task<UserResponse> SoftDeleteUserAsync(int id);
        Task<(bool IsSuccess, string ResultOrError)> UpdateUserProfileAsync(int id, UpdateUserProfileRequest request);
    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Not found this userId");

            return new PhyGen.Services.Object.Responses.UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                EmailVerifiedAt = user.EmailVerifiedAt,
                LastLoginAt = user.LastLoginAt,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };   
        }
        public async Task<UserResponse> SoftDeleteUserAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.SoftDeleteUserAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("User is already soft deleted or not found.");
            }

           int affectedRows = await _unitOfWork.SaveChangesWithTransactionAsync();
            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Failed to update user status in database");
            }

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                IsActive = user.IsActive ?? false,
                EmailVerifiedAt = user.EmailVerifiedAt,
                LastLoginAt = user.LastLoginAt,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
        public async Task<(bool IsSuccess, string ResultOrError)> UpdateUserProfileAsync(int id, UpdateUserProfileRequest request)
        {
            if (request == null)
            {
                return (false, "Update data is required");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return (false, $"User with ID {id} not found");
            }

            // Chỉ cập nhật các field có giá trị từ request
            if (!string.IsNullOrEmpty(request.FullName))
            {
                user.FullName = request.FullName;
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                user.Email = request.Email;
            }
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                user.PhoneNumber = request.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(request.AvatarUrl))
            {
                user.AvatarUrl = request.AvatarUrl;
            }

            user.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.UserRepository.Update(user); // Đảm bảo EF Core theo dõi
            // Lưu thay đổi với transaction
            int affectedRows = await _unitOfWork.SaveChangesWithTransactionAsync();
            if (affectedRows == 0)
            {
                return (false, "Failed to update user profile");
            }

            return (true, "User profile updated successfully");
        }
        //
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
           return await _unitOfWork.UserRepository.GetAllUserAsync() ?? throw new KeyNotFoundException("No user in database");
        }

    }
}
