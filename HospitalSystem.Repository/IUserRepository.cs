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

        public Task<bool> UpdateUserAsync(User updatedUser);

        public Task<bool> ChangePasswordAsync(int UserId, string newPassword);

        public Task<User> Find(int UserId);
    }
}
