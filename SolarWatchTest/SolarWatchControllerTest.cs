using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Services;
using SolarWatch.Services.CityServices.Repository;
using SolarWatch.Services.Jsonprocessor;
using SolarWatch.Services.SolarDataServices.Repository;

namespace SolarWatchTest;

[TestFixture]
public class SolarWatchControllerTest
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ILocationDataProvider> _locationDataProviderMock;
    private Mock<ILocationJsonProcessor> _locationJsonProcessorMock;
    private Mock<ISolarDataProvider> _solarDataProviderMock;
    private Mock<ISolarJsonProcessor> _solarJsonProcessorMock;
    private Mock<ICityRepository> _cityRepositoryMock;
    private Mock<ISolarDataRepository> _solarDataRepositoryMock;
    private SolarWatchController _controller;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _locationDataProviderMock = new Mock<ILocationDataProvider>();
        _locationJsonProcessorMock = new Mock<ILocationJsonProcessor>();
        _solarDataProviderMock = new Mock<ISolarDataProvider>();
        _solarJsonProcessorMock = new Mock<ISolarJsonProcessor>();
        _cityRepositoryMock = new Mock<ICityRepository>();
        _solarDataRepositoryMock = new Mock<ISolarDataRepository>();

        _controller = new SolarWatchController(
            _loggerMock.Object,
            _locationDataProviderMock.Object,
            _locationJsonProcessorMock.Object,
            _solarDataProviderMock.Object,
            _solarJsonProcessorMock.Object,
            _cityRepositoryMock.Object,
            _solarDataRepositoryMock.Object
        );
    }

    [Test]
    public async Task GetReturnsNotFoundWhenCityDoesNotExistAndLocationDataProviderFails()
    {
        _cityRepositoryMock.Setup(x => x.CheckIfCityExists(It.IsAny<string>())).ReturnsAsync(false);
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).Throws(new Exception());

        var result = await _controller.Get(DateOnly.FromDateTime(DateTime.Now), "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetReturnsNotFoundWhenLocationJsonProcessorFails()
    {
        _cityRepositoryMock.Setup(x => x.CheckIfCityExists(It.IsAny<string>())).ReturnsAsync(false);
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).ReturnsAsync("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Throws(new Exception());

        var result = await _controller.Get(DateOnly.FromDateTime(DateTime.Now), "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetReturnsNotFoundWhenSolarDataProviderFails()
    {
        _cityRepositoryMock.Setup(x => x.CheckIfCityExists(It.IsAny<string>())).ReturnsAsync(false);
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).ReturnsAsync("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(new City());
        _solarDataProviderMock.Setup(x => x.GetSolarForecastAsync(It.IsAny<City>(), It.IsAny<DateOnly>()))
            .Throws(new Exception());

        var result = await _controller.Get(DateOnly.FromDateTime(DateTime.Now), "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetReturnsNotFoundWhenSolarJsonProcessorFails()
    {
        _cityRepositoryMock.Setup(x => x.CheckIfCityExists(It.IsAny<string>())).ReturnsAsync(false);
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).ReturnsAsync("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(new City());
        _solarDataProviderMock.Setup(x => x.GetSolarForecastAsync(It.IsAny<City>(), It.IsAny<DateOnly>()))
            .ReturnsAsync("{}");
        _solarJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<City>())).Throws(new Exception());

        var result = await _controller.Get(DateOnly.FromDateTime(DateTime.Now), "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetReturnsOkWhenAllDependenciesSucceed()
    {
        var expectedSolarData = new SolarData();
        _cityRepositoryMock.Setup(x => x.CheckIfCityExists(It.IsAny<string>())).ReturnsAsync(false);
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).ReturnsAsync("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(new City());
        _solarDataProviderMock.Setup(x => x.GetSolarForecastAsync(It.IsAny<City>(), It.IsAny<DateOnly>()))
            .ReturnsAsync("{}");
        _solarJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<City>())).Returns(expectedSolarData);

        var result = await _controller.Get(DateOnly.FromDateTime(DateTime.Now), "sampleCity");
        
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(expectedSolarData));
    }
}