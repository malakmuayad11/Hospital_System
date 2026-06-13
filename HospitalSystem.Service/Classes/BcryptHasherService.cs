using HospitalSystem.Service.Interfaces;

namespace HospitalSystem.Service.Classes
{
    public class BcryptHasherService : IPasswordHasherService
    {
        private const int _workFactor = 12;

        public string ComputeHash(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password, _workFactor);

        public bool Verify(string password, string hashedPassword) => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
