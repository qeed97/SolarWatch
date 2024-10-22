using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatchIntegrationTest;

[Collection("IntegrationTests")]
public class SolarWatchControllerIntegrationTest
{
    private readonly SolarWatchWebApplicationFactory _app;
    private readonly HttpClient _client;
    
    public SolarWatchControllerIntegrationTest()
    {
        _app = new SolarWatchWebApplicationFactory();
        _client = _app.CreateClient();
    }

    [Fact]
    public async Task TestEndPoint()
    {
        var cityName = "Miskolc";
        var date = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        
        var response = await _client.GetAsync($"/SolarWatch/GetSolarWatch?date={date:yyyy-MM-dd}&cityName={cityName}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var solarWatch = JsonSerializer.Deserialize<SolarData>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(solarWatch);
        Assert.Equal(date, solarWatch.Date);
        Assert.Equal(new TimeOnly(6, 0), solarWatch.Sunrise);
        Assert.Equal(new TimeOnly(18, 0), solarWatch.Sunset);
    }
}