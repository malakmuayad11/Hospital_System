using HospitalSystem.Infrastructure.DTOs.AuthenticationDTOs;

namespace HospitalSystem.Service.Validation
{
    public class UsersTokensValidation
    {
        public static bool ValidateLoginRequestDto(LoginRequestDto loginRequestDto) =>
            !string.IsNullOrEmpty(loginRequestDto.Username) &&
            !string.IsNullOrEmpty(loginRequestDto.Password);

        public static bool ValidateRefreshRequestDto(RefreshRequestDto refreshRequestDto) =>
            !string.IsNullOrEmpty(refreshRequestDto.RefreshToken) &&
            !string.IsNullOrEmpty(refreshRequestDto.Username);

        public static bool ValidateLogoutRequestDto(LogoutRequestDto logoutRequestDto) =>
            !string.IsNullOrEmpty(logoutRequestDto.Username) &&
            !string.IsNullOrEmpty(logoutRequestDto.RefreshToken);

    }
}
