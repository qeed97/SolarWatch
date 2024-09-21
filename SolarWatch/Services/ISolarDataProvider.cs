using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ISolarDataProvider
{ 
    Task<String> GetSolarForecastAsync(City city, DateTime date);
}