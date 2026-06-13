using HospitalSystem.Infrastructure.DTOs.Users;

namespace HospitalSystem.Infrastructure.DTOs.UserDTOs
{
    public class LoginUserDto : UserDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
