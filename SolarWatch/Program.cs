using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Data;
using SolarWatch.Services;
using SolarWatch.Services.Authentication;
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
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISolarDataRepository, SolarDataRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ILocationDataProvider, OpenWeatherMapApi>();
builder.Services.AddSingleton<ILocationJsonProcessor, LocationJsonProcessor>();
builder.Services.AddSingleton<ISolarDataProvider, SolarDataProvider>();
builder.Services.AddSingleton<ISolarJsonProcessor, SolarJsonProcessor>();
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["ValidIssuer"],
            ValidAudience = jwtSettings["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["IssuerSigningKey"]))
        };
    });

builder.Services.AddDbContext<UsersContext>(options =>
{
    options.UseSqlServer(
        $"Server=solar-watch,1433;Database=solar-watch;User Id=sa;Password=Hurkakolbasz24?;Encrypt=false;");
});

builder.Services
    .AddIdentityCore<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<UsersContext>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();