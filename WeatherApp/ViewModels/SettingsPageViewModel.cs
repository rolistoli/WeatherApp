using CommunityToolkit.Mvvm.Input;
using WeatherApp.Helpers;

namespace WeatherApp.ViewModels;

public sealed partial class SettingsPageViewModel : BaseViewModel
{
    public SettingsPageViewModel()
    {
    }

    [RelayCommand]
    private void SetLightTheme()
    {
        AppSettings.SetTheme(ThemeMode.Light);
    }

    [RelayCommand]
    private void SetDarkTheme()
    {
        AppSettings.SetTheme(ThemeMode.Dark);
    }

}
