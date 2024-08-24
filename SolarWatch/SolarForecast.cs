namespace SolarWatch;

public class SolarForecast
{
    public DateOnly Date { get; set; }

    public TimeOnly Sunrise { get; set; }

    public TimeOnly SunSet { get; set; }
    
    public string City { get; set; }
}