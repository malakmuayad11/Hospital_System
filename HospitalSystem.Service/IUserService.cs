using HospitalSystem.DTOs;

namespace HospitalSystem.Service
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllUsersAsync();

        public Task<int> GetUsersCountAsync();

        public Task<bool> AddUserAsync(AddUserDto userDto);

        public Task<bool> UpdateUserAsync(UpdateUserDto userDto);

        public Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        public Task<UserDto> FindAsync(int UserId);

        public Task<UserDto> FindAsync(FindUserDto findUserDto);

        public Task<bool> IsUsernameUsedAsync(string Username);

        public Task<bool> DeleteUserAsync(int UserId);
    }
}
