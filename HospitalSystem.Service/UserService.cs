using HospitalSystem.API.Models;
using HospitalSystem.DTOs;
using HospitalSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;

        public UserService(IUserRepository UserRepository)
        {
            this._UserRepository = UserRepository;
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            List<UserDto> UserDtos = new List<UserDto>();

            foreach (User User in await _UserRepository.GetAllUsersAsync())
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

        public async Task<int> GetUsersCountAsync() => await _UserRepository.GetUsersCountAsync();

        public async Task<bool> AddUserAsync(AddUserDto addUserDto)
        {
            User user = new User
            {
                Username = addUserDto.Username,
                Password = addUserDto.Password,
                Role = addUserDto.Role,
                Permissions = addUserDto.Permissions
            };

            return await _UserRepository.AddUserAsync(user);
        }

        public async Task<bool?> UpdateUserAsync(UpdateUserDto userDto) =>
            await _UserRepository.UpdateUserAsync(new User
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
                Role = userDto.Role,
                Permissions = userDto.Permissions
            });

        // Must implement hashing 
        public async Task<bool?> ChangePasswordAsync(ChangePasswordDto changePasswordDto) =>
            await _UserRepository.ChangePasswordAsync(changePasswordDto.UserId, changePasswordDto.Password);

        public async Task<UserDto> FindAsync(int UserId)
        {
            User user = await _UserRepository.FindAsync(UserId);

            if (user == null)
                return null;

            return new UserDto(
                    user.UserId,
                    user.Username,
                    user.Role,
                    user.LastLoginDate,
                    user.Permissions);
        }

        public async Task<UserDto> FindAsync(FindUserDto findUserDto)
        {
            User user = await _UserRepository.FindAsyc(findUserDto.Username, findUserDto.Password);

            if (user == null)
                return null;

            return new UserDto(
                    user.UserId,
                    user.Username,
                    user.Role,
                    user.LastLoginDate,
                    user.Permissions);
        }

        public async Task<bool> IsUsernameUsedAsync(string Username) =>
            await _UserRepository.IsUsernameUsedAsync(Username);

        public async Task<bool?> DeleteUserAsync(int UserId) =>
            await _UserRepository.DeleteUserAsync(UserId);
    }
}
