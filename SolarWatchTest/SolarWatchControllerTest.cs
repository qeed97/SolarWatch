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
    public void GetReturnsNotFoundResultWhenLocationDataProviderFails()
    {
        _locationDataProviderMock.Setup(x => x.GetLocation(It.IsAny<string>())).Throws(new Exception());

        var result = _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public void GetReturnsNotFoundResultWhenLocationJsonProcessorFails()
    {
        _locationDataProviderMock.Setup(x => x.GetLocation(It.IsAny<string>())).Returns("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Throws(new Exception());

        var result = _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public void GetReturnsNotFoundResultWhenSolarDataProviderFails()
    {
        _locationDataProviderMock.Setup(x => x.GetLocation(It.IsAny<string>())).Returns("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(new LocationCoordinates());
        _solarDataProviderMock.Setup(x => x.GetSolarForecast(It.IsAny<LocationCoordinates>(), It.IsAny<DateTime>()))
            .Throws(new Exception());

        var result = _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public void GetReturnsNotFoundResultWhenSolarJsonProcessorFails()
    {
        _locationDataProviderMock.Setup(x => x.GetLocation(It.IsAny<string>())).Returns("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(new LocationCoordinates());
        _solarDataProviderMock.Setup(x => x.GetSolarForecast(It.IsAny<LocationCoordinates>(), It.IsAny<DateTime>()))
            .Returns("{}");
        _solarJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Throws(new Exception());

        var result = _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public void GetReturnsOkResultWhenAllProvidersAndProcessorsSucceed()
    {
        var expectedForecast = new SolarForecast();
        _locationDataProviderMock.Setup(x => x.GetLocation(It.IsAny<string>())).Returns("{}");
        _locationJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(new LocationCoordinates());
        _solarDataProviderMock.Setup(x => x.GetSolarForecast(It.IsAny<LocationCoordinates>(), It.IsAny<DateTime>()))
            .Returns("{}");
        _solarJsonProcessorMock.Setup(x => x.Process(It.IsAny<string>())).Returns(expectedForecast);


        var result = _controller.Get(DateTime.Now, "sampleCity");
        
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(expectedForecast));
    }
}