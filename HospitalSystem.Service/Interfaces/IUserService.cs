using HospitalSystem.Infrastructure.DTOs.UserDTOs;
using HospitalSystem.Infrastructure.DTOs.Users;

namespace HospitalSystem.Service.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllUsersAsync();

        public Task<int> GetUsersCountAsync();

        public Task<int?> AddUserAsync(AddUserDto userDto);

        public Task<bool?> UpdateUserAsync(UpdateUserDto userDto);

        public Task<bool?> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        public Task<UserDto> FindAsync(int UserId);

        public Task<UserDto> FindAsync(FindUserDto findUserDto);

        public Task<LoginUserDto> FindAsync(string username);

        public Task<bool> IsUsernameUsedAsync(string username);

        public Task<bool?> DeleteUserAsync(int userId);

        public Task<bool?> UpdateUserLastLoginDateAsync(int userId);

        public Task<bool?> AddAsCurrentUserAsync(int userId);
    }
}
