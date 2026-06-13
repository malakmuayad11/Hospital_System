using HospitalSystem.Infrastructure.DTOs.UsersTokensDTOs;

namespace HospitalSystem.Service.Interfaces
{
    public interface IUsersTokensService
    {
        public Task<bool?> LoginAsync(int userId, string refreshToken, DateTime refreshTokenExpiresAt);

        public Task<TokenForUserDto> GetTokenDataForUserAsync(int userId);

        public Task<bool?> RefreshAsync(int userId, string refreshToken, DateTime refreshTokenExpiresAt);

        public Task<string> GetRefreshTokenHashForUserAsync(int userId);

        public Task<bool?> LogoutAsync(int userId, DateTime refreshTokenRevokedAt);
    }
}
