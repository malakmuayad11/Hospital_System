using HospitalSystem.Service.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HospitalSystem.Service.Classes
{
    public class NotepadLoggerService : ILoggerService
    {
        private string _logFilePath;
        private string _logsDirectory;
        private readonly IConfiguration _configuration;

        public NotepadLoggerService(IConfiguration configuration)
        {
            _configuration = configuration;
            _Initialize();
        }

        private void _Initialize()
        {
            _logsDirectory = _configuration["LogsFilePath"];

            if (!Directory.Exists(_logsDirectory))
                Directory.CreateDirectory(_logsDirectory);

            _logFilePath = Path.Combine(_logsDirectory, "HospitalSystemApiLogs.txt");
        }

        public async Task Log(string message, string ip, string userId)
        {
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logMessage = $"{timeStamp} [{message}] -> IP: {ip}, UserID: {userId}\n";
            await File.AppendAllTextAsync(_logFilePath, logMessage);
        }
    }
}
