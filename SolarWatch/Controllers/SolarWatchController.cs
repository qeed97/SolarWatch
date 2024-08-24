using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;
using SolarWatch.Services.Jsonprocessor;

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

    public SolarWatchController(ILogger<SolarWatchController> logger,ILocationDataProvider locationDataProvider, ILocationJsonProcessor locationJsonProcessor, ISolarDataProvider solarDataProvider, ISolarJsonProcessor solarJsonProcessor)
    {
        _logger = logger;
        _locationDataProvider = locationDataProvider;
        _locationJsonProcessor = locationJsonProcessor;
        _solarDataProvider = solarDataProvider;
        _solarJsonProcessor = solarJsonProcessor;
    }

    [HttpGet("GetSolarWatch")]
    public ActionResult<SolarForecast> Get(DateTime date, string city)
    {
        // TODO: MAKE CITY REQUIRED (MAYBE DATE TOO)
        try
        {
            var locationJson = _locationDataProvider.GetLocation(city);
            var locationData = _locationJsonProcessor.Process(locationJson);
            var solarJson = _solarDataProvider.GetSolarForecast(locationData, date);
            return Ok(_solarJsonProcessor.Process(solarJson));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting solar data");
            return NotFound("Error getting solar data");
        }
        
    }
}