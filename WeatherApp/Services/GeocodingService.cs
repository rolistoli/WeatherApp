using System.Net.Http.Json;
using WeatherApp.Constants;
using WeatherApp.DTOs;
// using WeatherApp.Mappers;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services;

public sealed class GeocodingService(HttpClient httpClient) : IGeocodingService
{
    public async Task<IReadOnlyList<CityLocation>> SearchAsync(
        string name,
        int count = 10,
        string language = "en",
        string format = "json",
        CancellationToken cancellationToken = default)
    {
        // if (string.IsNullOrWhiteSpace(name))
        // {
        //     throw new ArgumentException("Name parameter is required.", nameof(name));
        // }
        //
        // if (count <= 0)
        // {
        //     throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero.");
        // }
        //
        // var nameValue = Uri.EscapeDataString(name);
        // var languageValue = Uri.EscapeDataString(language);
        // var formatValue = Uri.EscapeDataString(format);
        // var requestUri = $"{ApiConstants.GeocodingSearchEndpoint}?name={nameValue}&count={count}&language={languageValue}&format={formatValue}";
        //
        // using var response = await httpClient.GetAsync(requestUri, cancellationToken);
        // response.EnsureSuccessStatusCode();
        //
        // var dto = await response.Content.ReadFromJsonAsync<GeocodingSearchResponseDto>()
        //     ?? throw new InvalidOperationException("Open-Meteo geocoding returned an empty response.");
        //
        // return WeatherMapper.ToLocations(dto);

        return null;
    }
}
