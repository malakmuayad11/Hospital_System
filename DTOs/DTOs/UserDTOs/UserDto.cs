namespace HospitalSystem.Infrastructure.DTOs.Users
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public byte Role { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int Permissions { get; set; }
    }
}
