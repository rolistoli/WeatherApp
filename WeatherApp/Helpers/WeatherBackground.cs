namespace WeatherApp.Helpers;

public static class WeatherBackground
{
    public static string GetBackgroundForCode(int code)
    {
        return code switch
        {
            0 => "weather_sunny.png",
            1 or 2 => "weather_partly_cloudy.png",
            3 or 45 or 48 => "weather_cloudy.png",
            51 or 53 or 55 or 56 or 61 or 63 or 65 or 80 or 81 or 82 => "weather_rain.png",
            71 or 73 or 75 or 77 or 85 or 86 => "weather_snow.png",
            95 or 96 or 99 => "weather_thunder.png",
            _ => "weather_unknown.png"
        };
    }
}
