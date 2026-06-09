namespace WeatherApp.Models;
public sealed class PointWeather
{
    public double Temperature { get; init; }
    public double? RelativeHumidity { get; init; }
    public double? WindSpeed { get; init; }
    public int WeatherCode { get; init; }
    public string Time { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
