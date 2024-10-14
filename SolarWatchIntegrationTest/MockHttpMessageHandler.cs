using System.Net;

namespace SolarWatchIntegrationTest;

public class MockHttpMessageHandler : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Mock response for the Sunrise-Sunset API
        if (request.RequestUri.ToString().Contains("sunrise-sunset.org"))
        {
            var responseContent = @"{ ""results"": { ""sunrise"": ""6:00 AM"", ""sunset"": ""7:00 PM"" }, ""status"": ""OK"" }";
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };
        }

        // Mock response for OpenWeatherMap API
        if (request.RequestUri.ToString().Contains("openweathermap.org"))
        {
            var responseContent = @"[{ ""name"": ""London"", ""lat"": 51.5074, ""lon"": -0.1278,""country"": United Kingdom, ""state"": England }]";
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };
        }

        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }
}
