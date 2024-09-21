using SolarWatch.Models;

namespace SolarWatch.Services.CityServices.Repository;

public interface ICityRepository
{
    public Task<bool> CheckIfCityExists(string cityName);
    public void CreateCity(City city);
    public IEnumerable<City> GetCities();
    public ValueTask<City?> GetCityByName(string name);
    public City UpdateCity(City city);
    public void DeleteCity(City city);
}