using SolarWatch.Models;

namespace SolarWatch.Services.Jsonprocessor;

public interface ILocationJsonProcessor
{
    City Process(string data);
}