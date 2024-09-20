using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Services.CityServices.Repository;

public class CityRepository(SolarDbContext context) : ICityRepository
{
    public void CreateCity(City city)
    {
        context.Cities.Add(city);
        context.SaveChanges();
    }

    public IEnumerable<City> GetCities()
    {
        return context.Cities
            .Include(c => c.SolarData);
    }

    public async ValueTask<City?> GetCityByName(string name)
    {
        return await context.Cities
            .Include(c => c.SolarData)
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public City UpdateCity(City city)
    {
        context.Update(city);
        context.SaveChanges();
        return new City
        {
            Id = city.Id, Name = city.Name, SolarData = city.SolarData,
            Country = city.Country, 
            Latitude = city.Latitude, Longitude = city.Longitude, 
            State = city.State
        };
    }

    public void DeleteCity(City city)
    {
        context.Cities.Remove(city);
        context.SaveChanges();
    }
}