namespace SolarWatch.Models;

public class UpdatedSolarData
{
    public DateOnly Date { get; set; }
    public TimeOnly Sunrise { get; set; }
    public TimeOnly Sunset { get; set; }
}