using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using StockScreener.Data;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "19IGo9YUtDpou4H9pWA8Yhro9PzP0hEO"; // Replace with your Polygon.io API key

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _httpClient = new HttpClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<string> stockSymbols = new() { "AAPL", "GOOGL", "MSFT", "TSLA", "AMZN" }; // Add more stocks here

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Fetching stock data for multiple stocks...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StockScreenerDbContext>();

                var fetchTasks = stockSymbols.Select(symbol => FetchStockDataAsync(symbol)).ToArray();
                var stockDataList = await Task.WhenAll(fetchTasks);

                var validStockData = stockDataList.Where(stock => stock != null).ToList();

                if (validStockData.Any())
                {
                    dbContext.StockPrices.AddRange(validStockData);
                    await dbContext.SaveChangesAsync();
                }
            }

            _logger.LogInformation("Stock data updated. Sleeping for 10 minutes...");
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }


    private async Task<StockPrice> FetchStockDataAsync(string symbol)
    {
        try
        {
            var url = $"https://api.polygon.io/v2/aggs/ticker/{symbol}/prev?apiKey={_apiKey}";

            var response = await _httpClient.GetStringAsync(url);
            var jsonDoc = JsonDocument.Parse(response);

            if (jsonDoc.RootElement.TryGetProperty("results", out JsonElement results) && results.GetArrayLength() > 0)
            {
                var latest = results.EnumerateArray().First(); // Get the most recent entry
                var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(latest.GetProperty("t").GetInt64()).UtcDateTime;
                var price = latest.GetProperty("c").GetDecimal(); // Closing price (latest)

                return new StockPrice
                {
                    Symbol = symbol,
                    Price = price,
                    Timestamp = timestamp
                };
            }

            _logger.LogWarning("No data found for symbol: {Symbol}", symbol);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching stock data for symbol: {Symbol}", symbol);
            return null;
        }
    }
}
