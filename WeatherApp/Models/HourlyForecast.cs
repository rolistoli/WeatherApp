namespace WeatherApp.Models;

public sealed class HourlyForecast
{
    public string Time { get; init; } = string.Empty;
    public double Temperature { get; init; }
    public int WeatherCode { get; init; }
    public string Description { get; init; } = string.Empty;
    public string TemperatureDisplay => $"{Temperature:F1} °C";
    public string BackgroundImageName { get; init; } = string.Empty;
    public string HourLabel
    {
        get
        {
            if (DateTime.TryParse(Time, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out var dt))
            {
                return dt.ToString("HH:mm");
            }

            return Time;
        }
    }
}
