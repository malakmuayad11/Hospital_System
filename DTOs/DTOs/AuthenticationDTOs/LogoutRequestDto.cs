namespace HospitalSystem.Infrastructure.DTOs.AuthenticationDTOs
{
    public class LogoutRequestDto
    {
        public string Username { get; set; }
        public string RefreshToken { get; set; }
    }
}
