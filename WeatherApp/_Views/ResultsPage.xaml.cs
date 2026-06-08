using WeatherApp.Helpers;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class ResultsPage : ContentPage
{
    public const string Route = "results";
    private readonly ToolbarItem settingsToolbarItem;

    public ResultsPage(ResultsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        settingsToolbarItem = new ToolbarItem();
        settingsToolbarItem.Clicked += OnSettingsToolbarItemClicked;
        ToolbarItems.Add(settingsToolbarItem);

        LocalizedStrings.Current.PropertyChanged += OnLocalizedStringsChanged;
        UpdateLocalizedText();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateSettingsIcon();
        _ = PageTransitionAnimations.PlayEntranceAsync(PageContent);
    }

    private async void OnSettingsToolbarItemClicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NavigationPage(new SettingsPage()), true);
    }

    private void OnLocalizedStringsChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        UpdateLocalizedText();
    }

    private void UpdateLocalizedText()
    {
        Title = LocalizedStrings.Current["ResultsPageTitle"];
        UpdateSettingsIcon();
        DataSourceLabel.Text = LocalizedStrings.Current["DataSource"];
        LoadingWeatherLabel.Text = LocalizedStrings.Current["LoadingWeather"];
        RetryButton.Text = LocalizedStrings.Current["Retry"];
        NowLabel.Text = LocalizedStrings.Current["Now"];
        FiveDayForecastLabel.Text = LocalizedStrings.Current["FiveDayForecast"];
    }

    private void UpdateSettingsIcon()
    {
        settingsToolbarItem.IconImageSource = null;
        settingsToolbarItem.Text = "⚙";
    }
}
