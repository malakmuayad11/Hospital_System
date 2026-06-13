namespace HospitalSystem.Infrastructure.DTOs.AuthenticationDTOs
{
    public class RefreshRequestDto
    {
        public string RefreshToken { get; set; }
        public string Username { get; set; }
    }
}
