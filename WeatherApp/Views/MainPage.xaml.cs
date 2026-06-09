namespace WeatherApp.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private async void OnAddToolbarItemClicked(object? sender, EventArgs e)
    {
        //await Navigation.PushModalAsync(new NavigationPage(new SettingsPage()), true);
    }
    private async void OnSettingsToolbarItemClicked(object? sender, EventArgs e)
    {
        //await Navigation.PushModalAsync(new NavigationPage(new SettingsPage()), true);
    }
}