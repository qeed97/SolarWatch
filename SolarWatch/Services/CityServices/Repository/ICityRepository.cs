using SolarWatch.Models;

namespace SolarWatch.Services.CityServices.Repository;

public interface ICityRepository
{
    public Task<bool> CheckIfCityExists(string cityName);
    public Task<City?> GetCityById(int cityId);
    public void CreateCity(City city);
    public IEnumerable<City> GetCities();
    public ValueTask<City?> GetCityByName(string name);
    public City UpdateOldCity(City city, UpdatedCity updatedCity);
    public City UpdateCity(City city);
    public void DeleteCity(City city);
}