namespace WeatherApp.Models;
public sealed class DailyForecast
{
    public string Date { get; init; } = string.Empty;
    public string DateValue { get; init; } = string.Empty;
    public int WeatherCode { get; init; }
    public double TemperatureMin { get; init; }
    public double TemperatureMax { get; init; }
    public string Description { get; init; } = string.Empty;
    public string TemperatureRange { get; init; } = string.Empty;
}
