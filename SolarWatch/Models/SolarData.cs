using System.Text.Json.Serialization;

namespace SolarWatch.Models;

public class SolarData
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Sunrise { get; set; }
    public TimeOnly Sunset { get; set; }
    [JsonIgnore]
    public City City { get; set; }
    public int CityId { get; set; }
}