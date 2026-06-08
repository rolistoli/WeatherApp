using System.Text.Json.Serialization;

namespace WeatherApp.DTOs;

public sealed class WeatherForecastResponseDto
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; init; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationTimeMs { get; init; }

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; init; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; init; } = string.Empty;

    [JsonPropertyName("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; init; } = string.Empty;

    [JsonPropertyName("elevation")]
    public double Elevation { get; init; }

    [JsonPropertyName("current")]
    public CurrentWeatherDto Current { get; init; } = new();

    [JsonPropertyName("daily")]
    public DailyWeatherDto Daily { get; init; } = new();
}

public sealed class CurrentWeatherDto
{
    [JsonPropertyName("time")]
    public string Time { get; init; } = string.Empty;

    [JsonPropertyName("interval")]
    public int? Interval { get; init; }

    [JsonPropertyName("temperature_2m")]
    public double? Temperature2m { get; init; }

    [JsonPropertyName("weather_code")]
    public int? WeatherCode { get; init; }
}

public sealed class DailyWeatherDto
{
    [JsonPropertyName("time")]
    public List<string> Time { get; init; } = [];

    [JsonPropertyName("weather_code")]
    public List<int> WeatherCode { get; init; } = [];

    [JsonPropertyName("temperature_2m_max")]
    public List<double> Temperature2mMax { get; init; } = [];

    [JsonPropertyName("temperature_2m_min")]
    public List<double> Temperature2mMin { get; init; } = [];
}
