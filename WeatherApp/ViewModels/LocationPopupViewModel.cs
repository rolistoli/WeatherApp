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

    [ObservableProperty]
    private bool isVisible;

    [RelayCommand]
    private void Add()
    {
        // placeholder for add action
        IsVisible = false;
    }

    [RelayCommand]
    private async Task Close()
    {
        // If this popup was shown as a modal page, act like the back button and pop the modal.
        var navigation = Application.Current?.Windows.FirstOrDefault()?.Page?.Navigation;
        if (navigation is not null && navigation.ModalStack.Count > 0)
        {
            try
            {
                await navigation.PopModalAsync();
                return;
            }
            catch
            {
                // fall through to hiding the popup if pop fails
            }
        }

        // otherwise just hide the overlay popup
        IsVisible = false;
    }

    public CommunityToolkit.Mvvm.Input.IAsyncRelayCommand DetailsCommand => new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(async () =>
    {
        if (Location is null) return;

        try
        {
            IsLoading = true;
            await navigationService.GoToResultsAsync(Location);
            IsVisible = false;
        }
        finally
        {
            IsLoading = false;
        }
    });

    public async Task BindLocationAsync(CityLocation loc)
    {
        // show loading state and initialize values on UI thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Location = loc;
            // Do not show raw coordinates while loading; show a loading title instead
            Title = string.Empty;
            Subtitle = string.Empty;
            Temperature = string.Empty;
            Description = string.Empty;
            IsVisible = true;
            IsLoading = true;
            WeatherImage = string.Empty;
        });

        // reverse geocode for nicer name; prefer Portuguese
        try
        {
            var zone = await geocodingService.ReverseAsync(loc.Latitude, loc.Longitude, language: "pt");
            if (zone is not null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // replace the location with service-provided info
                    Location = zone;
                    Title = zone.Name ?? Title;
                    Subtitle = zone.Country ?? string.Empty;
                });
            }
        }
        catch { }

        try
        {
            var pw = await weatherService.GetPointWeatherAsync(loc.Latitude, loc.Longitude);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Temperature = $"{pw.Temperature:F1} °C";
                Description = pw.Description;
                // set image for the popup based on returned weather code
                WeatherImage = WeatherBackground.GetBackgroundForCode(pw.WeatherCode);
            });
        }
        catch { }
        finally
        {
            MainThread.BeginInvokeOnMainThread(() => IsLoading = false);
        }
    }
}
