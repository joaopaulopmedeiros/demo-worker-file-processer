namespace Demo;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

            string? filePath = Environment.GetEnvironmentVariable("INPUT_FILE_PATH");
            string? currentLine;

            if (File.Exists(filePath))
            {
                using (Stream stream = File.OpenRead(filePath))
                using (StreamReader reader = new StreamReader(stream))
                while ((currentLine = reader.ReadLine()) != null)
                {
                        _logger.LogInformation(currentLine);
                }
            } else
            {
                _logger.LogWarning($"Worker has not found input file {filePath}. Searching again on next iteration");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
