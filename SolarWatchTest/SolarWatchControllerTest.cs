using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch;
using SolarWatch.Controllers;
using SolarWatch.Services;
using SolarWatch.Services.Jsonprocessor;

namespace SolarWatchTest;

[TestFixture]
public class SolarWatchControllerTest
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ILocationDataProvider> _locationDataProviderMock;
    private Mock<ILocationJsonProcessor> _locationJsonProcessorMock;
    private Mock<ISolarDataProvider> _solarDataProviderMock;
    private Mock<ISolarJsonProcessor> _solarJsonProcessorMock;
    private SolarWatchController _controller;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _locationDataProviderMock = new Mock<ILocationDataProvider>();
        _locationJsonProcessorMock = new Mock<ILocationJsonProcessor>();
        _solarDataProviderMock = new Mock<ISolarDataProvider>();
        _solarJsonProcessorMock = new Mock<ISolarJsonProcessor>();
        _controller = new SolarWatchController(_loggerMock.Object, _locationDataProviderMock.Object, _locationJsonProcessorMock.Object,
            _solarDataProviderMock.Object, _solarJsonProcessorMock.Object);
    }

    [Test]
    public async Task GetReturnsNotFoundResultWhenLocationDataProviderFails()
    {
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).Throws(new Exception());

        var result = await _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetReturnsNotFoundResultWhenLocationJsonProcessorFails()
    {
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).ReturnsAsync("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Throws(new Exception());

        var result = await _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetReturnsNotFoundResultWhenSolarDataProviderFails()
    {
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).ReturnsAsync("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(new LocationCoordinates());
        _solarDataProviderMock.Setup(x => x.GetSolarForecastAsync(It.IsAny<LocationCoordinates>(), It.IsAny<DateTime>()))
            .Throws(new Exception());

        var result = await _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetReturnsNotFoundResultWhenSolarJsonProcessorFails()
    {
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).ReturnsAsync("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(new LocationCoordinates());
        _solarDataProviderMock.Setup(x => x.GetSolarForecastAsync(It.IsAny<LocationCoordinates>(), It.IsAny<DateTime>()))
            .ReturnsAsync("{}");
        _solarJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Throws(new Exception());

        var result = await _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task GetReturnsOkResultWhenAllProvidersAndProcessorsSucceed()
    {
        var expectedForecast = new SolarForecast();
        _locationDataProviderMock.Setup(x => x.GetLocationAsync(It.IsAny<string>())).ReturnsAsync("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(new LocationCoordinates());
        _solarDataProviderMock.Setup(x => x.GetSolarForecastAsync(It.IsAny<LocationCoordinates>(), It.IsAny<DateTime>()))
            .ReturnsAsync("{}");
        _solarJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(expectedForecast);


        var result = await _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(expectedForecast));
    }
}