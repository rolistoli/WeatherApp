using WeatherApp.DTOs;
using WeatherApp.Helpers;
using WeatherApp.Models;

namespace WeatherApp.Mappers;

public static class WeatherMapper
{
    public static IReadOnlyList<CityLocation> ToLocations(GeocodingSearchResponseDto dto)
    {
        return dto.Results
            .Where(result => !string.IsNullOrWhiteSpace(result.Name))
            .Select(result => new CityLocation
            {
                Id = result.Id,
                Name = result.Name,
                Latitude = result.Latitude,
                Longitude = result.Longitude,
                Country = result.Country,
                Admin1 = result.Admin1
            })
            .ToList();
    }

    public static WeatherForecast ToWeatherForecast(WeatherForecastResponseDto dto)
    {
        if (dto.Current.Temperature2m is null || dto.Current.WeatherCode is null)
        {
            throw new Exception("");
        }

        var daily = dto.Daily;
        var itemCount = new[]
        {
            daily.Time.Count,
            daily.WeatherCode.Count,
            daily.Temperature2mMax.Count,
            daily.Temperature2mMin.Count
        }.Min();

        if (itemCount == 0)
        {
            throw new Exception("");
        }

        var dailyForecasts = new List<DailyForecast>(itemCount);

        for (var index = 0; index < itemCount; index++)
        {
            var weatherCode = daily.WeatherCode[index];

            dailyForecasts.Add(new DailyForecast
            {
                Date = daily.Time[index],
                DateValue = daily.Time[index],
                WeatherCode = weatherCode,
                TemperatureMin = daily.Temperature2mMin[index],
                TemperatureMax = daily.Temperature2mMax[index],
                Description = WeatherCodeDescriptions.GetDescription(weatherCode),
                TemperatureRange = $"{daily.Temperature2mMin[index]:F0}°C / {daily.Temperature2mMax[index]:F0}°C"
            });
        }

        return new WeatherForecast
        {
            CurrentTemperature = dto.Current.Temperature2m.Value,
            CurrentWeatherCode = dto.Current.WeatherCode.Value,
            WeatherDescription = WeatherCodeDescriptions.GetDescription(dto.Current.WeatherCode.Value),
            DailyForecasts = dailyForecasts
        };
    }
}