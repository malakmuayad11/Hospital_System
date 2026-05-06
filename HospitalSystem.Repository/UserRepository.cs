using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using HospitalSystem.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HospitalSystemContext _Context;

        public UserRepository(HospitalSystemContext Context)
        {
            this._Context = Context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _Context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await _Context.Users
                .CountAsync();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            await _Context.Users.AddAsync(user);
            return await _Context.SaveChangesAsync() == 1;
        }

        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            User user = await _Context.Users
                .FindAsync(updatedUser.UserId);

            if (user == null)
                return false;

            user.Username = updatedUser.Username;
            user.Role = updatedUser.Role;
            user.Permissions = updatedUser.Permissions;

            return await _Context.SaveChangesAsync() == 1;
        }
    }
}
