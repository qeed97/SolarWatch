namespace SolarWatch.Services;

public interface ISolarDataProvider
{ 
    String GetSolarForecast(LocationCoordinates coordinates, DateTime date);
}