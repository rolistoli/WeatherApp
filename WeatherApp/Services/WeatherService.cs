using System.Globalization;
using System.Net.Http.Json;
using WeatherApp.Constants;
using WeatherApp.DTOs;
// using WeatherApp.Helpers;
// using WeatherApp.Mappers;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services;

public sealed class WeatherService(HttpClient httpClient) : IWeatherService
{
    public async Task<WeatherForecast> GetForecastAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        // var latitudeValue = latitude.ToString(CultureInfo.InvariantCulture);
        // var longitudeValue = longitude.ToString(CultureInfo.InvariantCulture);
        // var requestUri = $"{ApiConstants.ForecastEndpoint}?latitude={latitudeValue}&longitude={longitudeValue}&current=temperature_2m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min&forecast_days=5&timezone=auto";
        //
        // using var response = await httpClient.GetAsync(requestUri, cancellationToken);
        // response.EnsureSuccessStatusCode();
        //
        // var dto = await response.Content.ReadFromJsonAsync<WeatherForecastResponseDto>(cancellationToken)
        //     ?? throw new InvalidOperationException(LocalizedStrings.Current["WeatherEmptyResponse"]);
        //
        // return WeatherMapper.ToWeatherForecast(dto);
        
        return null;
    }
}
