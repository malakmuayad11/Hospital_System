namespace HospitalSystem.Infrastructure.DTOs.AuthenticationDTOs
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
