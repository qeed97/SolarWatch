using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Services.SolarDataServices.Repository;

public class SolarDataRepository(SolarDbContext context) : ISolarDataRepository
{
    public bool CheckIfSolarDataExists(DateOnly date, City city)
    {
        return city.SolarData.FirstOrDefault(x => x.Date == date) is not null;
    }
    public IEnumerable<SolarData> GetSolarData()
    {
        return context.SolarDatas
            .Include(s => s.City);
    }

    public Task<SolarData?> GetSolarDataById(int id)
    {
        return context.SolarDatas
            .Include(s => s.City)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public SolarData CreateSolarData(SolarData solarData, City city)
    {
        city.SolarData.Add(solarData);
        context.SolarDatas.Add(solarData);
        context.SaveChanges();
        return solarData;
    }

    public void DeleteSolarData(SolarData solarData, City city)
    {
        city.SolarData.Remove(solarData);
        context.SolarDatas.Remove(solarData);
        context.SaveChanges();
    }

    public SolarData UpdateSolarData(SolarData solarData)
    {
        context.Update(solarData);
        context.SaveChanges();
        return solarData;
    }

    public Task<SolarData?> GetSolarDataByDate(DateOnly date)
    {
        return context.SolarDatas
            .Include(s => s.City)
            .FirstOrDefaultAsync(s => s.Date == date);
    }
}