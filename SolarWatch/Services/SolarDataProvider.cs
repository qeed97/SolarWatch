using System.Net;

namespace SolarWatch.Services;

public class SolarDataProvider : ISolarDataProvider
{
    private readonly ILogger<SolarDataProvider> _logger;

    public SolarDataProvider(ILogger<SolarDataProvider> logger)
    {
        _logger = logger;
    }
    
    public string GetLocation(string city)
    {
        var apikey = Environment.GetEnvironmentVariable("APIKEY");
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apikey}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling OpenWeather API With url");

        return client.DownloadString(url);
    }
}