using Microsoft.AspNetCore.Mvc;
namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;

    public SolarWatchController(ILogger<SolarWatchController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetSolarWatch")]
    public ActionResult<SolarForecast> Get(DateTime date, string city)
    {
        var forecast = new SolarForecast
        {
            Date = DateOnly.FromDateTime(date),
            City = city,
            Sunrise = new TimeOnly(6, 30, 45),
            SunSet = new TimeOnly(20, 21, 32)
        };

        return Ok(forecast);
    }
}