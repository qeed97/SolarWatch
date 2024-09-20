using SolarWatch.Data;
using SolarWatch.Services;
using SolarWatch.Services.CityServices.Repository;
using SolarWatch.Services.Jsonprocessor;
using SolarWatch.Services.SolarDataServices.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DotNetEnv.Env.Load(".env");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SolarDbContext>();
builder.Services.AddSingleton<ICityRepository, CityRepository>();
builder.Services.AddSingleton<ISolarDataRepository, SolarDataRepository>();
builder.Services.AddSingleton<ILocationDataProvider, OpenWeatherMapApi>();
builder.Services.AddSingleton<ILocationJsonProcessor, LocationJsonProcessor>();
builder.Services.AddSingleton<ISolarDataProvider, SolarDataProvider>();
builder.Services.AddSingleton<ISolarJsonProcessor, SolarJsonProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();