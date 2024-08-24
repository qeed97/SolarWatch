namespace SolarWatch.Services;

public interface ISolarDataProvider
{ 
    String GetLocation(string city);
}