namespace HospitalSystem.Infrastructure.DTOs.Users
{
    public class FindUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; } = null!;
    }
}
