using System.Text.Json.Serialization;

namespace WeatherApp.DTOs;
public sealed class PointWeatherDto
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

    [JsonPropertyName("current_weather")]
    public CurrentPointWeatherDto CurrentWeather { get; init; } = new();
}

public sealed class CurrentPointWeatherDto
{
    [JsonPropertyName("temperature")]
    public double Temperature { get; init; }

    [JsonPropertyName("windspeed")]
    public double Windspeed { get; init; }

    [JsonPropertyName("winddirection")]
    public double WindDirection { get; init; }

    [JsonPropertyName("weathercode")]
    public int Weathercode { get; init; }

    [JsonPropertyName("time")]
    public string Time { get; init; } = string.Empty;
}
