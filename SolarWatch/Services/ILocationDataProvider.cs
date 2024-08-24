namespace SolarWatch.Services;

public interface ILocationDataProvider
{
    String GetLocation(string city);
}