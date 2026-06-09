namespace WeatherApp.Helpers;

public enum ThemeMode
{
    Light,
    Dark
}

public static class AppSettings
{
    private const string ThemePreferenceKey = "theme_mode";

    public static ThemeMode ThemeMode
    {
        get
        {
            var savedTheme = Preferences.Get(ThemePreferenceKey, ThemeMode.Light.ToString());
            return Enum.TryParse<ThemeMode>(savedTheme, out var themeMode)
                ? themeMode
                : ThemeMode.Light;
        }
    } 

    public static void ApplySavedPreferences()
    {
        ApplyTheme(ThemeMode);
    }

    public static void SetTheme(ThemeMode themeMode)
    {
        Preferences.Set(ThemePreferenceKey, themeMode.ToString());
        ApplyTheme(themeMode);
    }   

    private static void ApplyTheme(ThemeMode themeMode)
    {
        if (Application.Current is null)
        {
            return;
        }

        Application.Current.UserAppTheme = themeMode switch
        {
            ThemeMode.Dark => AppTheme.Dark,
            _ => AppTheme.Light
        };
    }
}
