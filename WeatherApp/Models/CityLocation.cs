namespace WeatherApp.Models;

public sealed class CityLocation
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string Country { get; init; } = string.Empty;
    public string Admin1 { get; init; } = string.Empty;
}
