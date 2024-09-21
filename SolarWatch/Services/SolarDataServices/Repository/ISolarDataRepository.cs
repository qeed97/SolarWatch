using SolarWatch.Models;

namespace SolarWatch.Services.SolarDataServices.Repository;

public interface ISolarDataRepository
{
    public bool CheckIfSolarDataExists(DateOnly date, City city);
    public IEnumerable<SolarData> GetSolarData();
    public Task<SolarData?> GetSolarDataById(int id);
    public SolarData CreateSolarData(SolarData solarData, City city);
    public void DeleteSolarData(SolarData solarData, City city);
    public SolarData UpdateSolarData(SolarData solarData);
    public Task<SolarData?> GetSolarDataByDate(DateOnly date);
}