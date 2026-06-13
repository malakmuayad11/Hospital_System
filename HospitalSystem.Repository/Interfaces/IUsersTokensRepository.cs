namespace HospitalSystem.Repository.Interfaces
{
    public interface IUsersTokensRepository
    {
        public Task<bool?> LoginAsync(int userId, string refreshTokenHash, DateTime refreshTokenExpiresAt);

        public Task<(DateTime? expiresAt, DateTime? revokedAt, string hash)> GetTokenDataForUserAsync(int userId);

        public Task<bool?> RefreshAsync(int userId, string refreshTokenHash, DateTime refreshTokenExpiresAt);

        public Task<string> GetRefreshTokenHashForUserAsync(int userId);

        public Task<bool?> LogoutAsync(int userId, DateTime refreshTokenRevokedAt);
    }
}
