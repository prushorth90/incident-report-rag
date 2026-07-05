using Microsoft.Extensions.DataIngestion;
using System.Diagnostics;
namespace IngestionService;

public class Worker(ILoggerFactory loggerFactory, ILogger<Worker> logger) : BackgroundService
{
    const string trackingFilePath = "tracking.txt";
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var directoryInfo = new DirectoryInfo("icm_incident_dataset");
        await File.Create(trackingFilePath).DisposeAsync();
        while (!stoppingToken.IsCancellationRequested)
        {
             var processedFiles =  (await File.ReadAllLinesAsync(trackingFilePath, stoppingToken)).ToHashSet();
            var filesToProcess = directoryInfo.EnumerateFiles("*.md").Where(file => !processedFiles.Contains(file.Name)).ToList();

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                logger.LogInformation("Files to process: {files}",
                    string.Join(", ", filesToProcess.Select(f => f.Name)));
            }

            var pipeline = new IngestionPipeline<string>(reader: new IcmReader(), chunker: null, writer: null, loggerFactory: loggerFactory);

            await foreach (var result in pipeline.ProcessAsync(filesToProcess, stoppingToken))
            {
                if (!result.Succeeded)
                {
                logger.LogError("Failed to process file: {file}", result.DocumentId);
                }
            }

            await File.AppendAllLinesAsync(trackingFilePath, filesToProcess.Select(f => f.Name), stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }

    public class IcmReader: IngestionDocumentReader 
    {

        private readonly MarkdownReader _markdownReader = new();
        public override Task<IngestionDocument> ReadAsync(Stream source, string identifier, string mediaType,   CancellationToken cancellationToken)
        {
           Debug.WriteLine(identifier);
           Debug.WriteLine(mediaType);
           return _markdownReader.ReadAsync(source, identifier, mediaType, cancellationToken);
        }
    }
}