using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class BcryptHasher : IPasswordHasher
    {
        private const int _workFactor = 12;

        public string HashPassword(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password, _workFactor);

        public bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
