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
            entity.HasKey(c => c.Id);
            
            entity.HasIndex(c => c.Id).IsUnique();
            
            entity.Property(c => c.Name).IsRequired();
            
            entity.Property(c => c.Country).IsRequired();

            entity.Property(c => c.State);
            
            entity.Property(c => c.Latitude).IsRequired();
            
            entity.Property(c => c.Longitude).IsRequired();
            
            entity.HasMany(c => c.SolarData)
                .WithOne(s => s.City)
                .HasForeignKey(s => s.City.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SolarData>(entity =>
        {
            entity.HasKey(s => s.Id);
            
            entity.HasIndex(s => s.Id).IsUnique();
            
            entity.Property(s => s.Date).IsRequired();
            
            entity.Property(s => s.Sunrise).IsRequired();
            
            entity.Property(s => s.Sunset).IsRequired();

            entity.HasOne(s => s.City)
                .WithMany(c => c.SolarData)
                .HasForeignKey(s => s.City.Id);
        });
    }
}