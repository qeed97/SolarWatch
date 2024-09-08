namespace SolarWatch.Services;

public interface ISolarDataProvider
{ 
    Task<String> GetSolarForecastAsync(LocationCoordinates coordinates, DateTime date);
}