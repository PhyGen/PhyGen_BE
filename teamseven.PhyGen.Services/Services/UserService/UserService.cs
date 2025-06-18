using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.UserService
{
    public interface IUserService
    {
        Task <IEnumerable<User>> GetUsersAsync ();
        Task <UserResponse> GetUserByIdAsync (int id);
        Task<UserResponse> SoftDeleteUserAsync(int id);
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

            // Lưu thay đổi với transaction
            await _unitOfWork.SaveChangesWithTransactionAsync();

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

        //
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
           return await _unitOfWork.UserRepository.GetAllUserAsync() ?? throw new KeyNotFoundException("No user in database");
        }

    }
}
