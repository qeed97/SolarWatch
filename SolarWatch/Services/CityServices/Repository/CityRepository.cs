﻿using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Services.CityServices.Repository;

public class CityRepository(SolarDbContext context) : ICityRepository
{

    public async Task<bool> CheckIfCityExists(string cityName)
    {
        if (context.Cities == null)
        {
            throw new InvalidOperationException("Database context is not initialized properly.");
        }
        return await context.Cities.FirstOrDefaultAsync(c => c.Name == cityName) != null;
    }
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

    public async Task<City?> GetCityById(int id)
    {
        return await context.Cities
            .Include(c => c.SolarData)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async ValueTask<City?> GetCityByName(string name)
    {
        return await context.Cities
            .Include(c => c.SolarData)
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public City UpdateOldCity(City city, UpdatedCity updatedCity)
    {
        city.Name = updatedCity.Name;
        city.Country = updatedCity.Country;
        city.Longitude = updatedCity.Longitude;
        city.Latitude = updatedCity.Latitude;
        return city;
    }
    
    public City UpdateCity(City city)
    {
        context.Update(city);
        context.SaveChanges();
        return new City
        {
            Id = city.Id, Name = city.Name, SolarData = city.SolarData,
            Country = city.Country, 
            Latitude = city.Latitude, Longitude = city.Longitude
        };
    }

    public void DeleteCity(City city)
    {
        context.Cities.Remove(city);
        context.SaveChanges();
    }
}