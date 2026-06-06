namespace HospitalSystem.DTOs.Users
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public byte Role { get; set; }

        public int Permissions { get; set; }

    }
}
