using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Lendship.Backend.Logger
{
    public class Logger : ILogger
    {
        private readonly string _logFolder;

        public Logger(IConfiguration _configuration)
        {
            _logFolder = _configuration.GetSection("Logging").GetValue("Folder", "C:\\LendshipLogs");
        }

        public void Error(string errorMsg)
        {
            if (!Directory.Exists(_logFolder))
            {
                Directory.CreateDirectory(_logFolder);
            }

            string fullPath = Path.Combine(_logFolder, DateTime.UtcNow.ToShortDateString() + ".txt");

            File.AppendAllText(fullPath, errorMsg + "\n");
        }
    }
}
