namespace HospitalSystem.Service.Interfaces
{
    public interface ILoggerService
    {
        public Task Log(string message, string ip, string userId);
    }
}
