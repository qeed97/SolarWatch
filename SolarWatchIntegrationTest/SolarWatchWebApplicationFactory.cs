using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SolarWatch.Data;
using SolarWatch.Models;
using SolarWatch.Services;

namespace SolarWatchIntegrationTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            var solarWatchDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarDbContext>));

            if (solarWatchDbContextDescriptor != null)
            {
                services.Remove(solarWatchDbContextDescriptor);
            }
            
            services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            services.AddDbContext<SolarDbContext>(options => options.UseInMemoryDatabase(_dbName));
            
            using var scope = services.BuildServiceProvider().CreateScope();

            var solarContext = scope.ServiceProvider.GetRequiredService<SolarDbContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();
            
            SeedData(solarContext);
        });
    }
    private void SeedData(SolarDbContext context)
    {
        var city = new City
        {
            Name = "Miskolc",
            Country = "Hungary",
            Latitude = 48.1030,
            Longitude = 20.7789,
            SolarData =
            [
                new SolarData
                {
                    Date = DateOnly.FromDateTime(DateTime.UtcNow.Date),
                    Sunrise = new TimeOnly(6, 0),
                    Sunset = new TimeOnly(18, 0),
                    City = new City
                    {
                        Name = "Miskolc",
                        Country = "Hungary",
                        Latitude = 48.1030,
                        Longitude = 20.7789
                    },
                    CityId = 1
                }
            ]
        };
       
        var sunriseSunset = new SolarData
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow.Date),
            Sunrise = new TimeOnly(6, 0),
            Sunset = new TimeOnly(18, 0),
            City =  new City{
                Name = "Miskolc",
                Country = "Hungary",
                Latitude = 48.1030,
                Longitude = 20.7789
            },
            CityId = 1
        };
        
        context.Cities.AddRange(city);
        context.SolarDatas.AddRange(sunriseSunset);
        
        context.SaveChanges();
    }
}