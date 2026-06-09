using System.Net.Http.Json;
using WeatherApp.Constants;
using WeatherApp.DTOs;
using WeatherApp.Mappers;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services;

public sealed class GeocodingService(HttpClient httpClient) : IGeocodingService
{
    public async Task<IReadOnlyList<CityLocation>> SearchAsync(
        string name,
        int count = 10,
        string language = "en",
        string format = "json")
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name parameter is required.", nameof(name));
        }

        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero.");
        }

        var nameValue = Uri.EscapeDataString(name);
        var languageValue = Uri.EscapeDataString(language);
        var formatValue = Uri.EscapeDataString(format);
        var requestUri = $"{ApiConstants.GeocodingSearchEndpoint}?name={nameValue}&count={count}&language={languageValue}&format={formatValue}";

        using var response = await httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<GeocodingSearchResponseDto>()
            ?? throw new InvalidOperationException("Open-Meteo geocoding returned an empty response.");

        return WeatherMapper.ToLocations(dto);
    }

    public async Task<CityLocation?> ReverseAsync(double latitude, double longitude, string language = "en")
    {
        // use Nominatim reverse to get display name and address fields; default language: Portuguese
        var latValue = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var lonValue = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

        var requestUri = $"https://nominatim.openstreetmap.org/reverse?lat={latValue}&lon={lonValue}&format=jsonv2&accept-language=pt";

        using var response = await httpClient.GetAsync(requestUri);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<WeatherApp.DTOs.NominatimReverseDto>();
        if (dto is null) return null;

        var name = dto.DisplayName ?? string.Empty;
        var city = dto.Address?.City ?? dto.Address?.Town ?? dto.Address?.Village ?? dto.Address?.County ?? string.Empty;

        return new CityLocation
        {
            Id = dto.PlaceId,
            Name = city ?? name,
            Latitude = double.TryParse(dto.Lat, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var latParsed) ? latParsed : latitude,
            Longitude = double.TryParse(dto.Lon, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lonParsed) ? lonParsed : longitude,
            Country = dto.Address?.Country ?? string.Empty,
            Admin1 = dto.Address?.State ?? string.Empty
        };
    }
}
