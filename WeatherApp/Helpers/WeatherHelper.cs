using System.Globalization;
using WeatherApp.Models;

namespace WeatherApp.Helpers;

public static class WeatherHelper
{
    public static string BuildLocationName(CityLocation location)
    {
        if (location is null) throw new ArgumentNullException(nameof(location));

        var parts = new[] { location.Name, location.Admin1, location.Country }
            .Where(part => !string.IsNullOrWhiteSpace(part));

        return string.Join(", ", parts);
    }

    public static string FormatDate(string value)
    {
        return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var date)
            ? date.ToString("ddd, dd MMM", CultureInfo.CurrentCulture)
            : value;
    }
}
