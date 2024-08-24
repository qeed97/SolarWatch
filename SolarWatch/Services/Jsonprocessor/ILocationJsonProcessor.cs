namespace SolarWatch.Services.Jsonprocessor;

public interface ILocationJsonProcessor
{
    LocationCoordinates Process(string data);
}