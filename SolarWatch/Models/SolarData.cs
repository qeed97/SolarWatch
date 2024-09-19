namespace SolarWatch.Models;

public class SolarData
{
    public int Id { get; set; }
    public TimeOnly Sunrise { get; set; }
    public TimeOnly Sunset { get; set; }
    public City City { get; set; }
}