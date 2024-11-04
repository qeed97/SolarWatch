using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Services;
using SolarWatch.Services.CityServices.Repository;
using SolarWatch.Services.Jsonprocessor;
using SolarWatch.Services.SolarDataServices.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ILocationDataProvider _locationDataProvider;
    private readonly ILocationJsonProcessor _locationJsonProcessor;
    private readonly ISolarDataProvider _solarDataProvider;
    private readonly ISolarJsonProcessor _solarJsonProcessor;
    private readonly ICityRepository _cityRepository;
    private readonly ISolarDataRepository _solarDataRepository;

    public SolarWatchController(ILogger<SolarWatchController> logger,ILocationDataProvider locationDataProvider, ILocationJsonProcessor locationJsonProcessor, ISolarDataProvider solarDataProvider, ISolarJsonProcessor solarJsonProcessor, ICityRepository cityRepository, ISolarDataRepository solarDataRepository)
    {
        _logger = logger;
        _locationDataProvider = locationDataProvider;
        _locationJsonProcessor = locationJsonProcessor;
        _solarDataProvider = solarDataProvider;
        _solarJsonProcessor = solarJsonProcessor;
        _cityRepository = cityRepository;
        _solarDataRepository = solarDataRepository;
    }

    [HttpGet("GetSolarWatch"), Authorize(Roles ="User, Admin")]
    public async Task<ActionResult<SolarData>> Get(DateOnly date, string cityName)
    {
        try
        {
            Console.WriteLine(date);
            Console.WriteLine(cityName);
            var city = new City();
            var solarData = new SolarData();
            if (!await _cityRepository.CheckIfCityExists(cityName))
            {
                var locationJson = await _locationDataProvider.GetLocationAsync(cityName);
                city = _locationJsonProcessor.Process(locationJson);
                _cityRepository.CreateCity(city);
                var solarJson = await _solarDataProvider.GetSolarForecastAsync(city, date);
                solarData = _solarJsonProcessor.Process(solarJson, date, city);
                _solarDataRepository.CreateSolarData(solarData, city);
                return Ok(solarData);
            }
            city = await _cityRepository.GetCityByName(cityName);
            if (!_solarDataRepository.CheckIfSolarDataExists(date, city))
            {
                var solarJson = await _solarDataProvider.GetSolarForecastAsync(city, date);
                solarData = _solarJsonProcessor.Process(solarJson, date, city);
                _solarDataRepository.CreateSolarData(solarData, city);
                return Ok(solarData);
            }
            solarData = await _solarDataRepository.GetSolarDataByDate(date);
            return Ok(solarData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting solar data");
            return NotFound("Error getting solar data");
        }
        
    }

    [HttpGet("GetExistingSolarWatch"), Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<IEnumerable<SolarData>>> GetExistingSolarWatch(string cityName)
    {
        try
        {
            var city = new City();
            if (!await _cityRepository.CheckIfCityExists(cityName))
            {
                return NotFound("City not found");
            }

            city = await _cityRepository.GetCityByName(cityName);
            var solarDatas = _solarDataRepository.GetSolarDataForCity(city);
            if (!solarDatas.Any())
            {
                return NotFound("No solar data found");
            }
            return Ok(solarDatas);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting solar data");
            return NotFound("Error getting solar data");
        }
    }

    [HttpGet("GetExistingCities"), Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<IEnumerable<City>>> GetExistingCities()
    {
        try
        {
            var cities = _cityRepository.GetCities();
            if (!cities.Any())
            {
                return NotFound("No cities found");
            }
            return Ok(cities);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting city data");
            return NotFound("Error getting city data");
        }
    }

    [HttpPut("UpdateSolarData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SolarData>> ChangeSolarData([FromBody] UpdatedSolarData updatedSolarData, int id)
    {
        try
        {
            var solarData = await _solarDataRepository.GetSolarDataById(id);
            if (solarData == null)
            {
                return NotFound("Solar data not found");
            }
            
            var updated = _solarDataRepository.UpdateOldSolarData(solarData, updatedSolarData);
            return Ok(_solarDataRepository.UpdateSolarData(updated));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error changing solar data");
            return NotFound("Error changing solar data");
        }
    }

    [HttpPut("UpdateCity"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<City>> UpdateCity([FromBody] UpdatedCity updatedCity, int id)
    {
        try
        {
            var city = await _cityRepository.GetCityById(id);
            if (city == null)
            {
                return NotFound("City not found");
            }
            
            var updated = _cityRepository.UpdateOldCity(city, updatedCity);
            return Ok(_cityRepository.UpdateCity(updated));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating city");
            return NotFound("Error updating city");
        }
    }

    [HttpDelete("DeleteSolarData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteSolarData(int id)
    {
        try
        {
            var solarData = await _solarDataRepository.GetSolarDataById(id);
            if (solarData == null)
            {
                return NotFound("Solar data not found");
            }
            _solarDataRepository.DeleteSolarData(solarData, solarData.City);
            return Ok("Solar data deleted");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting solar data");
            return NotFound("Error deleting solar data");
        }
    }

    [HttpDelete("DeleteCity"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCity(int id)
    {
        try
        {
            var city = await _cityRepository.GetCityById(id);
            if (city == null)
            {
                return NotFound("City not found");
            }
            _cityRepository.DeleteCity(city);
            return Ok("City deleted");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting city");
            return NotFound("Error deleting city");
        }
    }
}