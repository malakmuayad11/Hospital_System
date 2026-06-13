using HospitalSystem.API.Models;
using HospitalSystem.Data.Data;
using HospitalSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Repository.Classes
{
    public class UsersTokensRepository : IUsersTokensRepository
    {
        private readonly HospitalSystemContext _context;

        public UsersTokensRepository(HospitalSystemContext context) => _context = context;
        
        public async Task<bool?> LoginAsync(int userId, string refreshTokenHash, DateTime refreshTokenExpiresAt)
        {
            if (!await _context.Users.AnyAsync(u => u.UserId == userId))
                return null;

            UsersTokens usersTokens = new UsersTokens
            {
                UserId = userId,
                RefreshTokenHash = refreshTokenHash,
                RefreshTokenExpiresAt = refreshTokenExpiresAt,
                RefreshTokenRevokedAt = null
            };

            await _context.UsersTokens.AddAsync(usersTokens);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<(DateTime? expiresAt, DateTime? revokedAt, string hash)> GetTokenDataForUserAsync(int userId)
        {
            if (!await _context.Users.AnyAsync(u => u.UserId == userId))
                return (null, null, null);

            var tokenData = await _context.UsersTokens
                .Where(u => u.UserId == userId)
                .Select(ut => new { ut.RefreshTokenExpiresAt, ut.RefreshTokenRevokedAt, ut.RefreshTokenHash })
                .OrderByDescending(ut => ut.RefreshTokenExpiresAt)
                .FirstOrDefaultAsync();

            if (tokenData is null)
                return (null, null, null);

            return (tokenData.RefreshTokenExpiresAt, tokenData.RefreshTokenRevokedAt, tokenData.RefreshTokenHash);
        }

        public async Task<bool?> RefreshAsync(int userId, string refreshTokenHash, DateTime refreshTokenExpiresAt)
        {
            UsersTokens usersTokens = await _context.UsersTokens
                .OrderByDescending(ut => ut.RefreshTokenExpiresAt)
                .FirstOrDefaultAsync(ut => ut.UserId == userId);

            if (usersTokens is null)
                return null;

            usersTokens.RefreshTokenExpiresAt = refreshTokenExpiresAt;
            usersTokens.RefreshTokenHash = refreshTokenHash;
            usersTokens.RefreshTokenRevokedAt = null;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string> GetRefreshTokenHashForUserAsync(int userId) =>
            await _context.UsersTokens
                .Where(ut => ut.UserId == userId)
                .OrderByDescending(ut => ut.RefreshTokenExpiresAt)
                .Select(ut => ut.RefreshTokenHash)
                .FirstOrDefaultAsync();

        public async Task<bool?> LogoutAsync(int userId, DateTime refreshTokenRevokedAt)
        {
            UsersTokens usersTokens = await _context.UsersTokens
                .OrderByDescending(ut => ut.RefreshTokenExpiresAt)
                .FirstOrDefaultAsync(ut => ut.UserId == userId);

            if (usersTokens is null)
                return null;

            usersTokens.RefreshTokenRevokedAt = refreshTokenRevokedAt;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
