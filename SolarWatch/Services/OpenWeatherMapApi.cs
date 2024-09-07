using System.Net;

namespace SolarWatch.Services;

public class OpenWeatherMapApi : ILocationDataProvider
{
    private readonly ILogger<OpenWeatherMapApi> _logger;

    public OpenWeatherMapApi(ILogger<OpenWeatherMapApi> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> GetLocationAsync(string city)
    {
        DotNetEnv.Env.Load();
        var apikey = Environment.GetEnvironmentVariable("APIKEY");
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apikey}";

        using var client = new HttpClient();
        
        _logger.LogInformation("Calling OpenWeather API With url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}