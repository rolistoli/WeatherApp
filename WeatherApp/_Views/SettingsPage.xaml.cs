using WeatherApp.Helpers;

namespace WeatherApp.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        LocalizedStrings.Current.PropertyChanged += OnLocalizedStringsChanged;
        UpdateLocalizedText();
        UpdateControls();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = PageTransitionAnimations.PlayEntranceAsync(PageContent);
    }

    private void OnLightThemeTapped(object? sender, TappedEventArgs e)
    {
        AppSettings.SetTheme(ThemeMode.Light);
        UpdateControls();
    }

    private void OnDarkThemeTapped(object? sender, TappedEventArgs e)
    {
        AppSettings.SetTheme(ThemeMode.Dark);
        UpdateControls();
    }

    private void OnPortugueseLanguageTapped(object? sender, TappedEventArgs e)
    {
        AppSettings.SetLanguage("pt");
        UpdateControls();
    }

    private void OnEnglishLanguageTapped(object? sender, TappedEventArgs e)
    {
        AppSettings.SetLanguage("en");
        UpdateControls();
    }

    private async void OnCloseClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync(true);
    }

    private void UpdateControls()
    {
        UpdateOptionBorder(LightThemeOptionBorder, AppSettings.ThemeMode == ThemeMode.Light);
        UpdateOptionBorder(DarkThemeOptionBorder, AppSettings.ThemeMode == ThemeMode.Dark);
        UpdateOptionBorder(PortugueseLanguageOptionBorder, AppSettings.LanguageCode == "pt");
        UpdateOptionBorder(EnglishLanguageOptionBorder, AppSettings.LanguageCode == "en");
    }

    private void OnLocalizedStringsChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        UpdateLocalizedText();
    }

    private void UpdateLocalizedText()
    {
        Title = LocalizedStrings.Current["SettingsTitle"];
        SettingsTitleLabel.Text = LocalizedStrings.Current["SettingsTitle"];
        ThemeSectionLabel.Text = LocalizedStrings.Current["ThemeSection"];
        LightThemeLabel.Text = LocalizedStrings.Current["LightTheme"];
        DarkThemeLabel.Text = LocalizedStrings.Current["DarkTheme"];
        LanguageSectionLabel.Text = LocalizedStrings.Current["LanguageSection"];
        PortugueseLanguageLabel.Text = LocalizedStrings.Current["PortugueseLanguage"];
        EnglishLanguageLabel.Text = LocalizedStrings.Current["EnglishLanguage"];
        CloseButton.Text = LocalizedStrings.Current["Close"];
    }

    private static void UpdateOptionBorder(Border border, bool isSelected)
    {
        if (isSelected)
        {
            var selectedBackgroundColor = AppSettings.ThemeMode == ThemeMode.Dark
                ? Color.FromArgb("#512BD4")
                : Color.FromArgb("#DFD8F7");

            border.Stroke = new SolidColorBrush(Color.FromArgb("#512BD4"));
            border.BackgroundColor = selectedBackgroundColor;
            border.StrokeThickness = 2;
            return;
        }

        border.Stroke = new SolidColorBrush(Color.FromArgb("#C8C8C8"));
        border.BackgroundColor = Application.Current?.UserAppTheme == AppTheme.Dark
            ? Color.FromArgb("#212121")
            : Colors.White;
        border.StrokeThickness = 1;
    }
}
