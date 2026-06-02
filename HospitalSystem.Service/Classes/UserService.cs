using HospitalSystem.API.Models;
using HospitalSystem.DTOs.Users;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository UserRepository) => this._userRepository = UserRepository;
        public async Task<List<UserDto>> getAllUsersAsync()
        {
            List<UserDto> UserDtos = new List<UserDto>();

            foreach (User User in await _userRepository.GetAllUsersAsync())
            {
                UserDtos.Add(new UserDto(
                    User.UserId,
                    User.Username,
                    User.Role,
                    User.LastLoginDate,
                    User.Permissions));
            }

            // Indicates that there are no users in the database
            if (UserDtos.Count == 0)
                return null;

            return UserDtos;
        }

        public async Task<int> getUsersCountAsync() => await _userRepository.GetUsersCountAsync();

        public async Task<bool> addUserAsync(AddUserDto addUserDto)
        {
            User user = new User
            {
                Username = addUserDto.Username,
                Password = addUserDto.Password,
                Role = addUserDto.Role,
                Permissions = addUserDto.Permissions
            };

            return await _userRepository.AddUserAsync(user);
        }

        public async Task<bool?> updateUserAsync(UpdateUserDto userDto) =>
            await _userRepository.UpdateUserAsync(new User
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
                Role = userDto.Role,
                Permissions = userDto.Permissions
            });

        // Must implement hashing 
        public async Task<bool?> changePasswordAsync(ChangePasswordDto changePasswordDto) =>
            await _userRepository.ChangePasswordAsync(changePasswordDto.UserId, changePasswordDto.Password);

        public async Task<UserDto> findAsync(int userId)
        {
            User user = await _userRepository.FindAsync(userId);

            if (user == null)
                return null;

            return new UserDto(
                    user.UserId,
                    user.Username,
                    user.Role,
                    user.LastLoginDate,
                    user.Permissions);
        }

        public async Task<UserDto> findAsync(FindUserDto findUserDto)
        {
            User user = await _userRepository.FindAsyc(findUserDto.Username, findUserDto.Password);

            if (user == null)
                return null;

            return new UserDto(
                    user.UserId,
                    user.Username,
                    user.Role,
                    user.LastLoginDate,
                    user.Permissions);
        }

        public async Task<bool> isUsernameUsedAsync(string username) =>
            await _userRepository.IsUsernameUsedAsync(username);

        public async Task<bool?> deleteUserAsync(int userId) =>
            await _userRepository.DeleteUserAsync(userId);

        public async Task<bool?> updateUserLastLoginDateAsync(int userId) =>
            await _userRepository.UpdateUserLastLoginDateAsync(userId);

        public async Task<bool?> addAsCurrentUserAsync(int userId) =>
            await _userRepository.AddAsCurrentUserAsync(userId);
    }
}
