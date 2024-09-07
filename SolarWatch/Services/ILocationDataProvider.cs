namespace SolarWatch.Services;

public interface ILocationDataProvider
{
    Task<String> GetLocationAsync(string city);
}