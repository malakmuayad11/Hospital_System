using HospitalSystem.Data.Entities;
using HospitalSystem.Data.Data;
using HospitalSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repository.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly HospitalSystemContext _context;

        public UserRepository(HospitalSystemContext Context) => this._context = Context;

        public async Task<List<User>> GetAllUsersAsync() =>
            await _context.Users
                .AsNoTracking()
                .ToListAsync();

        public async Task<int> GetUsersCountAsync() => await _context.Users.CountAsync();

        public async Task<int?> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);

            if(await _context.SaveChangesAsync() > 0)
                return user.UserId;

            return null;
        }

        public async Task<bool?> UpdateUserAsync(User updatedUser)
        {
            User user = await this.FindAsync(updatedUser.UserId);

            if (user == null)
                return null;

            user.Username = updatedUser.Username;
            user.Role = updatedUser.Role;
            user.Permissions = updatedUser.Permissions;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?> ChangePasswordAsync(int userId, string newPassword)
        {
            User user = await this.FindAsync(userId);

            if (user == null)
                return null;

            user.Password = newPassword;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> FindAsync(int userId) =>
            await _context.Users
                .FindAsync(userId);

        public async Task<User> FindAsync(string username, string password) =>
            await _context.Users
                .SingleOrDefaultAsync(u => u.Username == username && u.Password == password);

        public async Task<User> FindAsync(string username) =>
            await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

        public async Task<bool> IsUsernameUsedAsync(string username) =>
            await _context.Users
            .AnyAsync(u => u.Username == username);

        public async Task<bool?> DeleteUserAsync(int userId)
        {
            User user = await this.FindAsync(userId);

            if (user == null)
                return null;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?> UpdateUserLastLoginDateAsync(int userId)
        {
            User user = await this.FindAsync(userId);

            if (user == null)
                return null;

            user.LastLoginDate = DateTime.UtcNow;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool?> AddAsCurrentUserAsync(int userId)
        {
            if (!await _context.Users
                .AnyAsync(u => u.UserId == userId))
                return null;

            SystemSetting systemSetting = await _context.SystemSettings
                .FindAsync(1);

            if (systemSetting == null)
                return false;

            systemSetting.CurrentUserId = userId;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
