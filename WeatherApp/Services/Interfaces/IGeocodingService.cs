using WeatherApp.Models;

namespace WeatherApp.Services.Interfaces;

public interface IGeocodingService
{
    Task<IReadOnlyList<CityLocation>> SearchAsync(
        string name,
        int count = 10,
        string language = "en",
        string format = "json");

    Task<CityLocation?> ReverseAsync(double latitude, double longitude, string language = "en");
}
