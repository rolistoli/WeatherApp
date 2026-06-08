namespace WeatherApp.Models;

public sealed class WeatherForecast
{
    public double CurrentTemperature { get; init; }

    public int CurrentWeatherCode { get; init; }

    public string WeatherDescription { get; init; } = string.Empty;

    public IReadOnlyList<DailyForecast> DailyForecasts { get; init; } = [];
}
