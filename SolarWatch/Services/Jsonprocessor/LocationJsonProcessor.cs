using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Services.Jsonprocessor;

public class LocationJsonProcessor : ILocationJsonProcessor
{
    public City Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement jsonElement = json.RootElement[0];

        City city = new City
        {
            Name = jsonElement.GetProperty("name").GetString(),
            Country = jsonElement.GetProperty("country").GetString(),
            Latitude = jsonElement.GetProperty("lat").GetDouble(),
            Longitude = jsonElement.GetProperty("lon").GetDouble(),
        };

        return city;
    }
}