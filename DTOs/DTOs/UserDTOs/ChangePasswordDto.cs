namespace HospitalSystem.Infrastructure.DTOs.Users
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; }
        public string Password { get; set; } = null!;
    }
}
