namespace WeatherApp.Models;

public sealed class WeatherForecast
{
    public double CurrentTemperature { get; init; }

    public int CurrentWeatherCode { get; init; }

    public string WeatherDescription { get; init; } = string.Empty;

    public IReadOnlyList<DailyForecast> DailyForecasts { get; init; } = [];
    public IReadOnlyList<HourlyForecast> HourlyForecasts { get; init; } = [];

    public string CurrentBackgroundImage { get; init; } = string.Empty;

}
