using HospitalSystem.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Service
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllUsersAsync();

        public Task<int> GetUsersCountAsync();

        public Task<bool> AddUserAsync(AddUserDto userDto);

        public Task<bool> UpdateUserAsync(UpdateUserDto userDto);

        public Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        public Task<UserDto> Find(int UserId);
    }
}
