using HospitalSystem.API.Models;
using HospitalSystem.Infrastructure.DTOs.UserDTOs;
using HospitalSystem.Infrastructure.DTOs.Users;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository UserRepository, IPasswordHasher passwordHasher)
        {
            this._userRepository = UserRepository;
            this._passwordHasher = passwordHasher;
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            List<UserDto> UserDtos = new List<UserDto>();

            foreach (User User in await _userRepository.GetAllUsersAsync())
            {
                UserDtos.Add(new UserDto {
                    UserId = User.UserId,
                    Username = User.Username,
                    Role = User.Role,
                    LastLoginDate = User.LastLoginDate,
                    Permissions = User.Permissions});
            }

            if (UserDtos.Count == 0)
                return null;

            return UserDtos;
        }

        public async Task<int> GetUsersCountAsync() => await _userRepository.GetUsersCountAsync();

        public async Task<int?> AddUserAsync(AddUserDto addUserDto)
        {
            User user = new User
            {
                Username = addUserDto.Username,
                Password = this._passwordHasher.HashPassword(addUserDto.Password),
                Role = addUserDto.Role,
                Permissions = addUserDto.Permissions
            };

            return await _userRepository.AddUserAsync(user);
        }

        public async Task<bool?> UpdateUserAsync(UpdateUserDto userDto) =>
            await _userRepository.UpdateUserAsync(new User
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
                Role = userDto.Role,
                Permissions = userDto.Permissions
            });

        public async Task<bool?> ChangePasswordAsync(ChangePasswordDto changePasswordDto) =>
            await _userRepository.ChangePasswordAsync(changePasswordDto.UserId, _passwordHasher.HashPassword(changePasswordDto.Password));

        private UserDto _MapUserDto(User user) =>
            new UserDto{
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role,
                LastLoginDate = user.LastLoginDate,
                Permissions = user.Permissions
            };

        public async Task<UserDto> FindAsync(int userId)
        {
            User user = await _userRepository.FindAsync(userId);

            if (user is null)
                return null;

            return _MapUserDto(user);
        }

        public async Task<UserDto> FindAsync(FindUserDto findUserDto)
        {
            User user = await _userRepository.FindAsync(findUserDto.Username);

            if (user is null)
                return null;

            if (!_passwordHasher.VerifyPassword(findUserDto.Password, user.Password))
                return null;

            return _MapUserDto(user);
        }

        public async Task<LoginUserDto> FindAsync(string username)
        {
            User user = await _userRepository.FindAsync(username);

            if (user is null)
                return null;

            return new LoginUserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                PasswordHash = user.Password,
                Role = user.Role,
                Permissions = user.Permissions
            };
        }

        public async Task<bool> IsUsernameUsedAsync(string username) =>
            await _userRepository.IsUsernameUsedAsync(username);

        public async Task<bool?> DeleteUserAsync(int userId) =>
            await _userRepository.DeleteUserAsync(userId);

        public async Task<bool?> UpdateUserLastLoginDateAsync(int userId) =>
            await _userRepository.UpdateUserLastLoginDateAsync(userId);

        public async Task<bool?> AddAsCurrentUserAsync(int userId) =>
            await _userRepository.AddAsCurrentUserAsync(userId);

        public static string RoleString(byte role)
        {
            switch (role)
            {
                case 1:
                    return "Admin";
                case 2:
                    return "Receptionist";
                case 3:
                    return "Doctor";
                default:
                    return "Unknown";
            }
        }

        public static string PermissionsString(int permissions) => permissions.ToString();

    }
}
