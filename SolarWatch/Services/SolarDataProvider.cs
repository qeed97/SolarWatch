﻿using System.Net;

namespace SolarWatch.Services;

public class SolarDataProvider : ISolarDataProvider
{
    private readonly ILogger<SolarDataProvider> _logger;

    public SolarDataProvider(ILogger<SolarDataProvider> logger)
    {
        _logger = logger;
    }
    
    public string GetSolarForecast(LocationCoordinates coordinates, DateTime date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={coordinates.Lat}&lng={coordinates.Lon}&date={date.Year}-{date.Month}-{date.Day}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling sunrire/sunset api with url: {url}", url);
        return client.DownloadString(url);
    }
}