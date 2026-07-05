namespace IngestionService;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var directoryInfo = new DirectoryInfo("icm_incident_dataset");

        while (!stoppingToken.IsCancellationRequested)
        {
            var filesToProcess = directoryInfo.EnumerateFiles("*.md");

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                logger.LogInformation("Files to process: {files}",
                    string.Join(", ", filesToProcess.Select(f => f.Name)));
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}