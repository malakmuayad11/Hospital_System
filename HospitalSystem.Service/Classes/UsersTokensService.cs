using HospitalSystem.Infrastructure.DTOs.UsersTokensDTOs;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class UsersTokensService : IUsersTokensService
    {
        private readonly IUsersTokensRepository _usersTokensRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public UsersTokensService(IUsersTokensRepository usersTokensRepository, IPasswordHasherService passwordHasherService)
        {
            _usersTokensRepository = usersTokensRepository;
            _passwordHasherService = passwordHasherService;
        }
        public Task<bool?> LoginAsync(int userId, string refreshToken, DateTime refreshTokenExpiresAt) =>
            _usersTokensRepository.LoginAsync(userId, _passwordHasherService.ComputeHash(refreshToken), refreshTokenExpiresAt);

        public async Task<TokenForUserDto> GetTokenDataForUserAsync(int userId)
        {
            (DateTime? expiresAt, DateTime? revokedAt, string hash)result = await _usersTokensRepository.GetTokenDataForUserAsync(userId);

            if (result is (null, null, null))
                return null;

            return new TokenForUserDto
            {
                ExpiresAt = result.expiresAt,
                RevokedAt = result.revokedAt,
                Hash = result.hash
            };
        }

        public async Task<bool?> RefreshAsync(int userId, string refreshToken, DateTime refreshTokenExpiresAt) =>
            await _usersTokensRepository.RefreshAsync(userId,  _passwordHasherService.ComputeHash(refreshToken), refreshTokenExpiresAt);

        public async Task<string> GetRefreshTokenHashForUserAsync(int userId) =>
            await _usersTokensRepository.GetRefreshTokenHashForUserAsync(userId);

        public async Task<bool?> LogoutAsync(int userId, DateTime refreshTokenRevokedAt) =>
            await _usersTokensRepository.LogoutAsync(userId, refreshTokenRevokedAt);
    }
}
