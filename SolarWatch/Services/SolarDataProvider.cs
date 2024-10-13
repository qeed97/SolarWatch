using System.Net;
using SolarWatch.Models;

namespace SolarWatch.Services;

public class SolarDataProvider : ISolarDataProvider
{
    private readonly ILogger<SolarDataProvider> _logger;

    public SolarDataProvider(ILogger<SolarDataProvider> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> GetSolarForecastAsync(City city, DateOnly date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={city.Latitude}&lng={city.Longitude}&date={date.Year}-{date.Month}-{date.Day}";

        using var client = new HttpClient();
        
        _logger.LogInformation("Calling sunrire/sunset api with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}