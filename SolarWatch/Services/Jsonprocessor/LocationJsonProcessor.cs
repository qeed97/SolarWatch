using System.Text.Json;

namespace SolarWatch.Services.Jsonprocessor;

public class LocationJsonProcessor : ILocationJsonProcessor
{
    public LocationCoordinates Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement jsonElement = json.RootElement[0];

        LocationCoordinates coordinates = new LocationCoordinates
        {
            Lat = jsonElement.GetProperty("lat").GetDouble(),
            Lon = jsonElement.GetProperty("lon").GetDouble(),
            Name = jsonElement.GetProperty("name").GetString()
        };

        return coordinates;
    }
}