using System.Text.Json;

namespace SolarWatch.Services.Jsonprocessor;

public class SolarJsonProcessor : ISolarJsonProcessor
{
    public SolarForecast Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement result = json.RootElement.GetProperty("results");

        // TODO:PARSE TIMES TO CORRECT REGION
        SolarForecast forecast = new SolarForecast
        {
            Sunrise = TimeOnly.Parse(result.GetProperty("sunrise").GetString(), System.Globalization.CultureInfo.InvariantCulture),
            SunSet = TimeOnly.Parse(result.GetProperty("sunset").GetString(), System.Globalization.CultureInfo.InvariantCulture)
        };

        return forecast;

    }
    
}