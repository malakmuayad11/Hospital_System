using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalSystem.API.Models;
using HospitalSystem.DTOs;

namespace HospitalSystem.Repository
{
    public interface IUserRepository
    {
        public Task<List<User>>GetAllUsersAsync();

        public Task<int> GetUsersCountAsync();

        public Task<bool> AddUserAsync(User user);

        public Task<bool?> UpdateUserAsync(User updatedUser);

        public Task<bool?> ChangePasswordAsync(int UserId, string newPassword);

        public Task<User> FindAsync(int UserId);

        public Task <User> FindAsyc (string Username, string password);

        public Task<bool> IsUsernameUsedAsync(string Username);

        public Task<bool?> DeleteUserAsync(int UserId);

        public Task<bool?> AddAsCurrentUserAsync(int UserId);

        public Task<bool?> UpdateUserLastLoginDateAsync(int UserId);
    }
}
