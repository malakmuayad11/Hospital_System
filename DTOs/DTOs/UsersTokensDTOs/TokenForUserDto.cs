namespace HospitalSystem.Infrastructure.DTOs.UsersTokensDTOs
{
    public class TokenForUserDto
    {
        public DateTime? ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string Hash { get; set; }
    }
}
