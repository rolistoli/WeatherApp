using System.Globalization;
using System.Net.Http.Json;
using WeatherApp.Constants;
using WeatherApp.DTOs;
using WeatherApp.Helpers;
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
        var nameValue = Uri.EscapeDataString(name);
        var languageValue = Uri.EscapeDataString(language);
        var formatValue = Uri.EscapeDataString(format);
        var requestUri = $"{ApiConstants.GeocodingSearchEndpoint}?name={nameValue}&count={count}&language={languageValue}&format={formatValue}";

        try
        {
            using var response = await httpClient.GetAsync(requestUri);
            await ApiErrorHandler.EnsureSuccessAsync(response);

            var dto = await response.Content.ReadFromJsonAsync<GeocodingSearchResponseDto>()
                ?? throw new InvalidOperationException("Empty response.");

            return WeatherMapper.ToLocations(dto);
        }

        catch (Exception ex) when (ex is not ApiException)
        {
            throw new ApiException("An error occurred while searching locations.", ex);
        }
    }

    public async Task<CityLocation?> ReverseAsync(double latitude, double longitude, string language = "en")
    {
        var latValue = latitude.ToString(CultureInfo.InvariantCulture);
        var lonValue = longitude.ToString(CultureInfo.InvariantCulture);

        var languageValue = Uri.EscapeDataString(language);
        var requestUri = $"{ApiConstants.NominatimReverseEndpoint}?lat={latValue}&lon={lonValue}&format=jsonv2&accept-language={languageValue}";

        try
        {
            using var response = await httpClient.GetAsync(requestUri);
            await ApiErrorHandler.EnsureSuccessAsync(response);

            var dto = await response.Content.ReadFromJsonAsync<NominatimReverseDto>()
                        ?? throw new InvalidOperationException("Empty response.");

            return WeatherMapper.ToLocation(dto, latitude, longitude);
        }
        catch (Exception ex) when (ex is not ApiException)
        {
            throw new ApiException("An error occurred while resolving coordinates to a location.", ex);
        }
    }
}
