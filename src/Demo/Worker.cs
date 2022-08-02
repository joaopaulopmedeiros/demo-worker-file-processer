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
            DateTimeOffset startAt = DateTimeOffset.Now;

            _logger.LogInformation($"Worker running at: {startAt}");

            string? filePath = Environment.GetEnvironmentVariable("INPUT_FILE_PATH");
            string? currentLine = null;
            int totalLines = 0;

            if (File.Exists(filePath))
            {
                using (Stream stream = File.OpenRead(filePath))
                using (StreamReader reader = new StreamReader(stream))
                while ((currentLine = reader.ReadLine()) != null)
                {
                    totalLines++;                    
                }
            } else
            {
                _logger.LogWarning($"Worker has not found input file {filePath}. Searching again on next iteration");
            }

            DateTimeOffset endAt = DateTimeOffset.Now;

            _logger.LogInformation($"Worker stopping at: {endAt}. It took {endAt.ToUnixTimeMilliseconds() - startAt.ToUnixTimeMilliseconds()} ms to read {totalLines} lines of file");

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
