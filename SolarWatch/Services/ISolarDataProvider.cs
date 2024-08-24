namespace SolarWatch.Services;

public interface ISolarDataProvider
{ 
    String GetSolarForecast(string city);
}