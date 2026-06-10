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
        var requestUri = $"{ApiConstants.OpenMeteoForecastEndpoint}?latitude={latitudeValue}&longitude={longitudeValue}&current=temperature_2m,weather_code&hourly=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min&forecast_days=5&timezone=auto";

        try
        {
            using var response = await httpClient.GetAsync(requestUri);
            await ApiErrorHandler.EnsureSuccessAsync(response);

            var dto = await response.Content.ReadFromJsonAsync<WeatherForecastResponseDto>()
                ?? throw new InvalidOperationException("Empty response");

            return WeatherMapper.ToWeatherForecast(dto);
        }
        catch (Exception ex) when (ex is not ApiException)
        {
            throw new ApiException("An error occurred while fetching the weather forecast.", ex);
        }
    }

    public async Task<PointWeather> GetPointWeatherAsync(
        double latitude,
        double longitude)
    {
        var latitudeValue = latitude.ToString(CultureInfo.InvariantCulture);
        var longitudeValue = longitude.ToString(CultureInfo.InvariantCulture);
        var requestUri = $"{ApiConstants.OpenMeteoForecastEndpoint}?latitude={latitudeValue}&longitude={longitudeValue}&current_weather=true&timezone=auto";

        try
        {
            using var response = await httpClient.GetAsync(requestUri);
            await ApiErrorHandler.EnsureSuccessAsync(response);

            var dto = await response.Content.ReadFromJsonAsync<PointWeatherDto>()
                ?? throw new InvalidOperationException("Empty response");

            return WeatherMapper.ToPointWeather(dto);
        }
        catch (Exception ex) when (ex is not ApiException)
        {
            throw new ApiException("An error occurred while fetching current point weather.", ex);
        }
    }
}
