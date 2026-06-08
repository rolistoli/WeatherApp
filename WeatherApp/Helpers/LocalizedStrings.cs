using System.ComponentModel;

namespace WeatherApp.Helpers;

public sealed class LocalizedStrings : INotifyPropertyChanged
{
    private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Strings =
        new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["pt"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["SettingsButton"] = "Configurações",
                ["SettingsTitle"] = "Configurações",
                ["ThemeSection"] = "Tema",
                ["LightTheme"] = "Claro",
                ["DarkTheme"] = "Escuro",
                ["LanguageSection"] = "Idioma",
                ["PortugueseLanguage"] = "Português",
                ["EnglishLanguage"] = "Inglês",
                ["Close"] = "Fechar",

                ["SearchPageTitle"] = "Pesquisar",
                ["SearchIntro"] = "Escreve pelo menos 3 letras e escolhe uma sugestão para veres a meteorologia.",
                ["SearchPlaceholder"] = "Pesquisar cidade",

                ["ResultsPageTitle"] = "Meteorologia",
                ["DataSource"] = "Dados fornecidos pela Open-Meteo",
                ["LoadingWeather"] = "A carregar meteorologia...",
                ["Retry"] = "Tentar novamente",
                ["Now"] = "Agora",
                ["FiveDayForecast"] = "Previsão de 5 dias",

                ["SearchNoSuggestions"] = "Não encontrei sugestões para essa cidade.",
                ["SearchBadRequest"] = "A pesquisa não foi aceite pela API. Confirma o texto.",
                ["SearchConnectionError"] = "Não foi possível ligar à API. Verifica a ligação à internet.",
                ["RequestTimeout"] = "A chamada demorou demasiado tempo. Tenta novamente.",
                ["SearchInvalidApiResponse"] = "A API devolveu uma resposta inválida.",
                ["SearchUnexpectedApiResponse"] = "A API devolveu uma resposta inesperada.",
                ["ResultsBadRequest"] = "A API recusou os dados da localização selecionada.",
                ["WeatherConnectionError"] = "Não foi possível obter a meteorologia. Verifica a ligação à internet.",
                ["WeatherInvalidApiData"] = "A API devolveu dados de meteorologia inválidos.",
                ["WeatherMissingCurrentTemperature"] = "A API não devolveu a temperatura atual.",
                ["WeatherMissingDailyForecast"] = "A API não devolveu previsão diária.",
                ["WeatherEmptyResponse"] = "A Open-Meteo devolveu uma resposta vazia."
            },
            ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["SettingsButton"] = "Settings",
                ["SettingsTitle"] = "Settings",
                ["ThemeSection"] = "Theme",
                ["LightTheme"] = "Light",
                ["DarkTheme"] = "Dark",
                ["LanguageSection"] = "Language",
                ["PortugueseLanguage"] = "Portuguese",
                ["EnglishLanguage"] = "English",
                ["Close"] = "Close",

                ["SearchPageTitle"] = "Search",
                ["SearchIntro"] = "Type at least 3 letters and choose a suggestion to see the weather.",
                ["SearchPlaceholder"] = "Search city",

                ["ResultsPageTitle"] = "Weather",
                ["DataSource"] = "Data provided by Open-Meteo",
                ["LoadingWeather"] = "Loading weather...",
                ["Retry"] = "Try again",
                ["Now"] = "Now",
                ["FiveDayForecast"] = "5-day forecast",

                ["SearchNoSuggestions"] = "I could not find suggestions for that city.",
                ["SearchBadRequest"] = "The API did not accept the search. Check the text.",
                ["SearchConnectionError"] = "Could not connect to the API. Check your internet connection.",
                ["RequestTimeout"] = "The request took too long. Try again.",
                ["SearchInvalidApiResponse"] = "The API returned an invalid response.",
                ["SearchUnexpectedApiResponse"] = "The API returned an unexpected response.",
                ["ResultsBadRequest"] = "The API rejected the selected location data.",
                ["WeatherConnectionError"] = "Could not get the weather. Check your internet connection.",
                ["WeatherInvalidApiData"] = "The API returned invalid weather data.",
                ["WeatherMissingCurrentTemperature"] = "The API did not return the current temperature.",
                ["WeatherMissingDailyForecast"] = "The API did not return the daily forecast.",
                ["WeatherEmptyResponse"] = "Open-Meteo returned an empty response."
            }
        };

    public static LocalizedStrings Current { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public string this[string key]
    {
        get
        {
            var languageCode = AppSettings.LanguageCode;

            if (Strings.TryGetValue(languageCode, out var languageStrings)
                && languageStrings.TryGetValue(key, out var text))
            {
                return text;
            }

            return Strings["pt"].TryGetValue(key, out var fallbackText) ? fallbackText : key;
        }
    }

    public void Refresh()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }
}
