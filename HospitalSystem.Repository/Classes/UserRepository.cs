using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using HospitalSystem.DTOs;
using HospitalSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.Repository.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly HospitalSystemContext _Context;

        public UserRepository(HospitalSystemContext Context)
        {
            this._Context = Context;
        }

        public async Task<List<User>> GetAllUsersAsync() =>
            await _Context.Users
                .AsNoTracking()
                .ToListAsync();

        public async Task<int> GetUsersCountAsync() => await _Context.Users.CountAsync();

        public async Task<int?> AddUserAsync(User user)
        {
            await _Context.Users.AddAsync(user);

            if(await _Context.SaveChangesAsync() == 1)
                return user.UserId;

            return null;
        }

        public async Task<bool?> UpdateUserAsync(User updatedUser)
        {
            User user = await _Context.Users
                .FindAsync(updatedUser.UserId);

            if (user == null)
                return null;

            user.Username = updatedUser.Username;
            user.Role = updatedUser.Role;
            user.Permissions = updatedUser.Permissions;

            return await _Context.SaveChangesAsync() == 1;
        }

        public async Task<bool?> ChangePasswordAsync(int UserId, string newPassword)
        {
            User user = await _Context.Users
                .FindAsync(UserId);

            if (user == null)
                return null;

            user.Password = newPassword;
            return await _Context.SaveChangesAsync() == 1;
        }

        public async Task<User> FindAsync(int UserId) =>
            await _Context.Users
                .FindAsync(UserId);

        public async Task<User> FindAsyc(string Username, string Password) =>
            await _Context.Users
                .SingleOrDefaultAsync(u => u.Username == Username && u.Password == Password);

        public async Task<bool> IsUsernameUsedAsync(string Username) =>
            await _Context.Users
            .AnyAsync(u => u.Username == Username);

        public async Task<bool?> DeleteUserAsync(int UserId)
        {
            User user = await _Context.Users
                .FindAsync(UserId);

            if (user == null)
                return null;

            _Context.Users.Remove(user);
            return await _Context.SaveChangesAsync() == 1;
        }

        public async Task<bool?> UpdateUserLastLoginDateAsync(int UserId)
        {
            User user = await _Context.Users
                .FindAsync(UserId);

            if (user == null)
                return null;

            user.LastLoginDate = DateTime.UtcNow;
            return await _Context.SaveChangesAsync() == 1;
        }

        public async Task<bool?> AddAsCurrentUserAsync(int UserId)
        {
            if (!await _Context.Users
                .AnyAsync(u => u.UserId == UserId))
                return null;

            SystemSetting systemSetting = await _Context.SystemSettings
                .FindAsync(1);

            if (systemSetting == null)
                return false;

            systemSetting.CurrentUserId = UserId;

            return await _Context.SaveChangesAsync() == 1;
        }
    }
}
