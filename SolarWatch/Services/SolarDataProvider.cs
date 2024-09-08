using System.Net;

namespace SolarWatch.Services;

public class SolarDataProvider : ISolarDataProvider
{
    private readonly ILogger<SolarDataProvider> _logger;

    public SolarDataProvider(ILogger<SolarDataProvider> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> GetSolarForecastAsync(LocationCoordinates coordinates, DateTime date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={coordinates.Lat}&lng={coordinates.Lon}&date={date.Year}-{date.Month}-{date.Day}";

        using var client = new HttpClient();
        
        _logger.LogInformation("Calling sunrire/sunset api with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}