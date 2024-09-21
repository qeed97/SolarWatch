namespace SolarWatch.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<SolarData> SolarData { get; set; }

    public City()
    {
        SolarData = new List<SolarData>();
    }
}