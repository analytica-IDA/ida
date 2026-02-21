using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{
    public interface IBackupService
    {
        string CreateBackup();
    }

    public class BackupService : IBackupService
    {
        private readonly IConfiguration _configuration;

        public BackupService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateBackup()
        {
            var fileName = $"backup_{DateTime.Now:yyyyMMddHHmmss}.sql";
            var backupPath = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
            if (!Directory.Exists(backupPath)) Directory.CreateDirectory(backupPath);

            var fullPath = Path.Combine(backupPath, fileName);

            // Execute pg_dump command via process
            // Note: In a real docker environment, this might need more setup
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "pg_dump",
                    Arguments = $"-h localhost -p 5434 -U ida_admin -d ida_db -f \"{fullPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                processInfo.EnvironmentVariables["PGPASSWORD"] = "ida_password";

                using var process = Process.Start(processInfo);
                process?.WaitForExit();
                return fullPath;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
