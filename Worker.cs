using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace TimeLoggerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int _intervalInMinutes;
        private static readonly string LogFilePath = Path.Combine(AppContext.BaseDirectory, "time_log.txt");

        public Worker(ILogger<Worker> logger, IOptions<appsettings> settings)
        {
            _logger = logger;
            _intervalInMinutes = settings.Value.IntervalInMinutes;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string logMessage = $"Worker running at: {DateTimeOffset.Now}";

                // Log to console/service log
                _logger.LogInformation(logMessage);

                // Optional: Log to file
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath)); // Ensure folder exists
                    await File.AppendAllTextAsync(LogFilePath, logMessage + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error writing to log file");
                }

                // Wait 5 minutes (300,000 milliseconds)
                await Task.Delay(TimeSpan.FromMinutes(_intervalInMinutes), stoppingToken);
            }
        }
    }
}