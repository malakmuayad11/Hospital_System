using HospitalSystem.API.Models;

namespace HospitalSystem.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsersAsync();

        public Task<int> GetUsersCountAsync();

        public Task<int?> AddUserAsync(User user);

        public Task<bool?> UpdateUserAsync(User updatedUser);

        public Task<bool?> ChangePasswordAsync(int userId, string newPassword);

        public Task<User> FindAsync(int userId);

        public Task <User> FindAsync (string username, string password);

        public Task<User> FindAsync(string username);

        public Task<bool> IsUsernameUsedAsync(string username);

        public Task<bool?> DeleteUserAsync(int userId);

        public Task<bool?> AddAsCurrentUserAsync(int userId);

        public Task<bool?> UpdateUserLastLoginDateAsync(int userId);
    }
}
