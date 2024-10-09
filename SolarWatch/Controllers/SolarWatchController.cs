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
    public async Task<ActionResult<SolarForecast>> Get(DateTime date, string cityName)
    {
        // TODO: MAKE CITY REQUIRED (MAYBE DATE TOO)
        try
        {
            var city = new City();
            var solarData = new SolarData();
            if (!await _cityRepository.CheckIfCityExists(cityName))
            {
                var locationJson = await _locationDataProvider.GetLocationAsync(cityName);
                city = _locationJsonProcessor.Process(locationJson);
                _cityRepository.CreateCity(city);
                var solarJson = await _solarDataProvider.GetSolarForecastAsync(city, date);
                solarData = _solarJsonProcessor.Process(solarJson, DateOnly.FromDateTime(date), city);
                _solarDataRepository.CreateSolarData(solarData, city);
                return Ok(new SolarForecast{Sunrise = solarData.Sunrise, SunSet = solarData.Sunset});
            }
            city = await _cityRepository.GetCityByName(cityName);
            if (!_solarDataRepository.CheckIfSolarDataExists(DateOnly.FromDateTime(date), city))
            {
                var solarJson = await _solarDataProvider.GetSolarForecastAsync(city, date);
                solarData = _solarJsonProcessor.Process(solarJson, DateOnly.FromDateTime(date), city);
                _solarDataRepository.CreateSolarData(solarData, city);
                return Ok(solarData);
            }
            solarData = await _solarDataRepository.GetSolarDataByDate(DateOnly.FromDateTime(date));
            return Ok(solarData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting solar data");
            return NotFound("Error getting solar data");
        }
        
    }
}