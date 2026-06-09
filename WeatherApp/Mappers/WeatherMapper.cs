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
            DailyForecasts = dailyForecasts,
            HourlyForecasts = ToHourlyForecasts(dto.Hourly),
            // add a background image for the current conditions too
            CurrentBackgroundImage = GetBackgroundForCode(dto.Current.WeatherCode.Value)
        };
    }

    private static IReadOnlyList<HourlyForecast> ToHourlyForecasts(HourlyWeatherDto hourly)
    {
        var count = new[] { hourly.Time.Count, hourly.Temperature2m.Count, hourly.WeatherCode.Count }.Min();
        var list = new List<HourlyForecast>(count);

        for (var i = 0; i < count; i++)
        {
            var code = hourly.WeatherCode[i];
            list.Add(new HourlyForecast
            {
                Time = hourly.Time[i],
                Temperature = hourly.Temperature2m[i],
                WeatherCode = code,
                Description = WeatherCodeDescriptions.GetDescription(code),
                BackgroundImageName = GetBackgroundForCode(code),
            });
        }
        return list;
    }

    private static string GetBackgroundForCode(int code)
    {
        // simple mapping: sunny, cloudy, rain, snow, thunder
        return code switch
        {
            0 => "weather_sunny.png",
            1 or 2 => "weather_partly_cloudy.png",
            3 or 45 or 48 => "weather_cloudy.png",
            51 or 53 or 55 or 56 or 61 or 63 or 65 or 80 or 81 or 82 => "weather_rain.png",
            71 or 73 or 75 or 77 or 85 or 86 => "weather_snow.png",
            95 or 96 or 99 => "weather_thunder.png",
            _ => "weather_unknown.png"
        };
    }
}