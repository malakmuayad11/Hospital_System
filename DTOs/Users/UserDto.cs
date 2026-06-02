namespace HospitalSystem.DTOs.Users
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public byte Role { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int Permissions { get; set; }

        public UserDto(int UserID, string Username, byte Role,
            DateTime? LastLoginDate, int Permissions)
        {
            this.UserID = UserID;
            this.Username = Username;
            this.Role = Role;
            this.LastLoginDate = LastLoginDate;
            this.Permissions = Permissions;
        }
    }
}
