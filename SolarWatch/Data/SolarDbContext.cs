using Microsoft.EntityFrameworkCore;
using System;
using SolarWatch.Models;

namespace SolarWatch.Data;

public class SolarDbContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    
    public DbSet<SolarData> SolarDatas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
        {
            connectionString =
                $"Server={Environment.GetEnvironmentVariable("LOCAL_SERVER_NAME")},{Environment.GetEnvironmentVariable("DB_PORT")};" +
                $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                $"User Id={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                $"Password={Environment.GetEnvironmentVariable("DB_USER_PASSWORD")};Encrypt=false;";
        }
        
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            
        });
    }
}