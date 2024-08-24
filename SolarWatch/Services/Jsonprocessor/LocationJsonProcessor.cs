using System.Text.Json;

namespace SolarWatch.Services.Jsonprocessor;

public class LocationJsonProcessor : ILocationJsonProcessor
{
    public LocationCoordinates Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);

        LocationCoordinates coordinates = new LocationCoordinates
        {
            Lat = json.RootElement.GetProperty("lat").GetDouble(),
            Lon = json.RootElement.GetProperty("lon").GetDouble(),
            Name = json.RootElement.GetProperty("name").GetString()
        };

        return coordinates;
    }
}