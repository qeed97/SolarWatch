namespace SolarWatch.Services.Jsonprocessor;

public interface ISolarJsonProcessor
{
    SolarForecast Process(string data);
}