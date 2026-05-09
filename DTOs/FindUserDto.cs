namespace HospitalSystem.DTOs
{
    public class FindUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; } = null!;
    }
}
