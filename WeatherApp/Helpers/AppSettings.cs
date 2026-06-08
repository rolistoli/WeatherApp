using System.Globalization;

namespace WeatherApp.Helpers;

public enum ThemeMode
{
    Light,
    Dark
}

public static class AppSettings
{
    private const string ThemePreferenceKey = "theme_mode";
    private const string LanguagePreferenceKey = "language_code";
    private const string PortugueseLanguageCode = "pt";
    private const string EnglishLanguageCode = "en";

    public static event EventHandler? LanguageChanged;

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

    public static string LanguageCode
    {
        get
        {
            var savedLanguage = Preferences.Get(LanguagePreferenceKey, PortugueseLanguageCode);
            return NormalizeLanguageCode(savedLanguage);
        }
    }

    public static void ApplySavedPreferences()
    {
        ApplyTheme(ThemeMode);
        ApplyCulture(LanguageCode);
        LocalizedStrings.Current.Refresh();
    }

    public static void SetTheme(ThemeMode themeMode)
    {
        Preferences.Set(ThemePreferenceKey, themeMode.ToString());
        ApplyTheme(themeMode);
    }

    public static void SetLanguage(string languageCode)
    {
        var normalizedLanguageCode = NormalizeLanguageCode(languageCode);

        if (LanguageCode == normalizedLanguageCode)
        {
            return;
        }

        Preferences.Set(LanguagePreferenceKey, normalizedLanguageCode);
        ApplyCulture(normalizedLanguageCode);
        LocalizedStrings.Current.Refresh();
        LanguageChanged?.Invoke(null, EventArgs.Empty);
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

    private static void ApplyCulture(string languageCode)
    {
        var culture = CultureInfo.GetCultureInfo(languageCode == EnglishLanguageCode ? "en-US" : "pt-PT");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    private static string NormalizeLanguageCode(string? languageCode)
    {
        return string.Equals(languageCode, EnglishLanguageCode, StringComparison.OrdinalIgnoreCase)
            ? EnglishLanguageCode
            : PortugueseLanguageCode;
    }
}
