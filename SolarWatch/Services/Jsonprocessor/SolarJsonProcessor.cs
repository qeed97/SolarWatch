using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Services.Jsonprocessor;

public class SolarJsonProcessor : ISolarJsonProcessor
{
    public SolarData Process(string data, DateOnly date, City city)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement result = json.RootElement.GetProperty("results");

        // TODO:PARSE TIMES TO CORRECT REGION
        SolarForecast forecast = new SolarForecast
        {
            Sunrise = TimeOnly.Parse(result.GetProperty("sunrise").GetString(), System.Globalization.CultureInfo.InvariantCulture),
            SunSet = TimeOnly.Parse(result.GetProperty("sunset").GetString(), System.Globalization.CultureInfo.InvariantCulture)
        };

        SolarData solarData = new SolarData
        {
            Sunrise = TimeOnly.Parse(result.GetProperty("sunrise").GetString(),
                System.Globalization.CultureInfo.InvariantCulture),
            Sunset = TimeOnly.Parse(result.GetProperty("sunset").GetString(),
                System.Globalization.CultureInfo.InvariantCulture),
            City = city,
            Date = date,
            CityId = city.Id
        };

        return solarData;

    }
    
}