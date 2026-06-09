using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
using Microsoft.Maui.ApplicationModel;
using WeatherApp.Helpers;

namespace WeatherApp.ViewModels;

public partial class LocationPopupViewModel : BaseViewModel
{
    private readonly IGeocodingService geocodingService;
    private readonly IWeatherService weatherService;
    private readonly INavigationService navigationService;

    public LocationPopupViewModel(IGeocodingService geocodingService, IWeatherService weatherService, INavigationService navigationService)
    {
        this.geocodingService = geocodingService;
        this.weatherService = weatherService;
        this.navigationService = navigationService;
    }

    [ObservableProperty]
    private CityLocation? location;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string subtitle = string.Empty;

    [ObservableProperty]
    private string temperature = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string weatherImage = string.Empty;

    [RelayCommand]
    private async Task Close()
    {
        var navigation = Application.Current?.Windows.FirstOrDefault()?.Page?.Navigation;
        if (navigation is not null && navigation.ModalStack.Count > 0)
        {
            try
            {
                await navigation.PopModalAsync(false);
                return;
            }
            catch
            {
                // fall through to hiding the popup if pop fails
            }
        }
    }

    [RelayCommand]
    private async Task Details()
    {
        if (Location is null) return;

        try
        {
            IsLoading = true;
 
            await navigationService.GoToResultsAsync(Location);
        }
        finally
        {
            // ensure the popup is closed before navigating to the results screen
            await Close();
            IsLoading = false;
        }
    }

    public async Task BindLocationAsync(CityLocation loc)
    {
        Location = loc;
        Title = string.Empty;
        Subtitle = string.Empty;
        Temperature = string.Empty;
        Description = string.Empty;
        IsLoading = true;
        WeatherImage = string.Empty;

        try
        {
            var zone = await geocodingService.ReverseAsync(
                loc.Latitude,
                loc.Longitude);

            if (zone is not null)
            {
                Location = zone;
                Title = zone.Name ?? string.Empty;
                Subtitle = zone.Country ?? string.Empty;
            }
        }
        catch
        {
        }

        try
        {
            var pw = await weatherService.GetPointWeatherAsync(
                loc.Latitude,
                loc.Longitude);

            Temperature = $"{pw.Temperature:F1} °C";
            Description = pw.Description;
            WeatherImage = WeatherBackground.GetBackgroundForCode(pw.WeatherCode);
        }
        catch
        {
        }
        finally
        {
            IsLoading = false;
        }
    }
}
