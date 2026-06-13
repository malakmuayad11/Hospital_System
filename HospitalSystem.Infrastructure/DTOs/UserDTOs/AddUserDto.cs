namespace HospitalSystem.Infrastructure.DTOs.Users
{
    public class AddUserDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public byte Role { get; set; }

        public int Permissions { get; set; }
    }
}
