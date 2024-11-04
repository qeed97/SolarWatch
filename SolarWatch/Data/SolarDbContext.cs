using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SolarWatch.Models;

namespace SolarWatch.Data;

public class SolarDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public SolarDbContext(DbContextOptions<SolarDbContext> options) : base(options) 
    {
    }
    public DbSet<City> Cities { get; set; }
    public DbSet<SolarData> SolarDatas { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(c => c.Id);
            
            entity.HasIndex(c => c.Id).IsUnique();
            
            entity.Property(c => c.Name).IsRequired();
            
            entity.Property(c => c.Country).IsRequired();
            
            entity.Property(c => c.Latitude).IsRequired();
            
            entity.Property(c => c.Longitude).IsRequired();
            
            entity.HasMany(c => c.SolarData)
                .WithOne(s => s.City)
                .HasForeignKey(s => s.CityId)
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
                .HasForeignKey(s => s.CityId);
        });
    }
}