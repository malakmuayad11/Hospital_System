using HospitalSystem.DTOs.Users;

namespace HospitalSystem.Service.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserDto>> getAllUsersAsync();

        public Task<int> getUsersCountAsync();

        public Task<bool> addUserAsync(AddUserDto userDto);

        public Task<bool?> updateUserAsync(UpdateUserDto userDto);

        public Task<bool?> changePasswordAsync(ChangePasswordDto changePasswordDto);

        public Task<UserDto> findAsync(int UserId);

        public Task<UserDto> findAsync(FindUserDto findUserDto);

        public Task<bool> isUsernameUsedAsync(string Username);

        public Task<bool?> deleteUserAsync(int UserId);

        public Task<bool?> updateUserLastLoginDateAsync(int UserId);

        public Task<bool?> addAsCurrentUserAsync(int UserId);
    }
}
