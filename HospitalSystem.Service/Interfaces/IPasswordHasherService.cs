namespace HospitalSystem.Service.Interfaces
{
    public interface IPasswordHasherService
    {
        public string ComputeHash(string password);
        public bool Verify(string password, string hashedPassword);
    }
}
