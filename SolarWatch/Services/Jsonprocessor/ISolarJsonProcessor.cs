using SolarWatch.Models;

namespace SolarWatch.Services.Jsonprocessor;

public interface ISolarJsonProcessor
{
    SolarData Process(string data, DateOnly date, City city);
}