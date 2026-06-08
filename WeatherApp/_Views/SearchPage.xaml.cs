using WeatherApp.Helpers;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

public partial class SearchPage : ContentPage
{
    public const string Route = "search";
    // private readonly ToolbarItem settingsToolbarItem;

    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        // settingsToolbarItem = new ToolbarItem();
        // settingsToolbarItem.Clicked += OnSettingsToolbarItemClicked;
        // ToolbarItems.Add(settingsToolbarItem);

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
        Title = LocalizedStrings.Current["SearchPageTitle"];
        UpdateSettingsIcon();
        IntroLabel.Text = LocalizedStrings.Current["SearchIntro"];
        CityEntry.Placeholder = LocalizedStrings.Current["SearchPlaceholder"];
    }

    private void UpdateSettingsIcon()
    {
        // settingsToolbarItem.IconImageSource = null;
        // settingsToolbarItem.Text = "⚙";
    }
}
