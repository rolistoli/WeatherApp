using System.Globalization;
using WeatherApp.DTOs;
using WeatherApp.Helpers;
using WeatherApp.Models;

namespace WeatherApp.Mappers;

public static class WeatherMapper
{
    public static IReadOnlyList<CityLocation> ToLocations(GeocodingSearchResponseDto dto)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

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
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        if (dto.Current is null)
        {
            throw new InvalidOperationException("Weather response is missing current weather information.");
        }

        if (dto.Current.Temperature2m is null || dto.Current.WeatherCode is null)
        {
            throw new InvalidOperationException("Current weather missing temperature or weather code.");
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
            throw new InvalidOperationException("Daily forecast data is empty.");
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
            CurrentBackgroundImage = WeatherBackground.GetBackgroundForCode(dto.Current.WeatherCode.Value)
        };
    }

    private static IReadOnlyList<HourlyForecast> ToHourlyForecasts(HourlyWeatherDto hourly)
    {
        if (hourly is null)
        {
            throw new ArgumentNullException(nameof(hourly));
        }
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
                BackgroundImageName = WeatherBackground.GetBackgroundForCode(code),
            });
        }
        return list;
    }  

    public static CityLocation ToLocation(NominatimReverseDto dto, double defaultLatitude, double defaultLongitude)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var name = dto.DisplayName ?? string.Empty;
        var city = dto.Address?.City ?? dto.Address?.Town ?? dto.Address?.Village ?? dto.Address?.County ?? string.Empty;

        var latitude = defaultLatitude;
        var longitude = defaultLongitude;

        if (double.TryParse(dto.Lat, NumberStyles.Any, CultureInfo.InvariantCulture, out var latParsed))
        {
            latitude = latParsed;
        }

        if (double.TryParse(dto.Lon, NumberStyles.Any, CultureInfo.InvariantCulture, out var lonParsed))
        {
            longitude = lonParsed;
        }

        return new CityLocation
        {
            Id = dto.PlaceId,
            Name = string.IsNullOrWhiteSpace(city) ? name : city,
            Latitude = latitude,
            Longitude = longitude,
            Country = dto.Address?.Country ?? string.Empty,
            Admin1 = dto.Address?.State ?? string.Empty
        };
    } 

    public static PointWeather ToPointWeather(PointWeatherDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var cw = dto.CurrentWeather ?? throw new InvalidOperationException("Point weather response is missing current weather.");

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