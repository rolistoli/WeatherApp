using WeatherApp.Models;

namespace WeatherApp.Services.Interfaces;

public interface IWeatherService
{
    Task<WeatherForecast> GetForecastAsync(
        double latitude,
        double longitude);

    Task<PointWeather> GetPointWeatherAsync(double latitude, double longitude);
}
