using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSystem.DTOs.Users
{
    public class AddUserDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public byte Role { get; set; }

        public int Permissions { get; set; }

        //public AddUserDto(string Username, string Password, byte Role, int Permissions)
        //{
        //    this.Username = Username;
        //    this.Password = Password;
        //    this.Role = Role;
        //    this.Permissions = Permissions;
        //}
    }
}
