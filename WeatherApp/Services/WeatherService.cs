using System.Globalization;
using System.Net.Http.Json;
using WeatherApp.Constants;
using WeatherApp.DTOs;
using WeatherApp.Helpers;
using WeatherApp.Mappers;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services;

public sealed class WeatherService(HttpClient httpClient) : IWeatherService
{
    public async Task<WeatherForecast> GetForecastAsync(
        double latitude,
        double longitude)
    {
        var latitudeValue = latitude.ToString(CultureInfo.InvariantCulture);
        var longitudeValue = longitude.ToString(CultureInfo.InvariantCulture);
        var requestUri = $"{ApiConstants.ForecastEndpoint}?latitude={latitudeValue}&longitude={longitudeValue}&current=temperature_2m,weather_code&hourly=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min&forecast_days=5&timezone=auto";

        using var response = await httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<WeatherForecastResponseDto>()
            ?? throw new InvalidOperationException("");

        return WeatherMapper.ToWeatherForecast(dto);
    }

    public async Task<PointWeather> GetPointWeatherAsync(double latitude, double longitude)
    {
        var latitudeValue = latitude.ToString(CultureInfo.InvariantCulture);
        var longitudeValue = longitude.ToString(CultureInfo.InvariantCulture);
        var requestUri = $"{ApiConstants.ForecastEndpoint}?latitude={latitudeValue}&longitude={longitudeValue}&current_weather=true&timezone=auto";

        using var response = await httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<PointWeatherDto>()
            ?? throw new InvalidOperationException("Failed to read point weather response");

        var cw = dto.CurrentWeather;

        return new PointWeather
        {
            Temperature = cw.Temperature,
            WindSpeed = cw.Windspeed,
            WeatherCode = cw.Weathercode,
            Time = cw.Time,
            Description = WeatherCodeDescriptions.GetDescription(cw.Weathercode)
        };
    }
}
