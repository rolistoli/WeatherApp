using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
using Microsoft.Maui.ApplicationModel;

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
    private bool isVisible;

    [RelayCommand]
    private void Add()
    {
        // placeholder for add action
        IsVisible = false;
    }

    [RelayCommand]
    private void Close()
    {
        IsVisible = false;
    }

    public IRelayCommand DetailsCommand => new RelayCommand(async () =>
    {
        if (Location is not null)
        {
            await navigationService.GoToResultsModalAsync(Location);
            IsVisible = false;
        }
    });

    public async Task BindLocationAsync(CityLocation loc)
    {
        // show loading state and initialize values on UI thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Location = loc;
            // Do not show raw coordinates while loading; show a loading title instead
            Title = "A carregar...";
            Subtitle = string.Empty;
            Temperature = string.Empty;
            Description = string.Empty;
            IsVisible = true;
            IsLoading = true;
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
            });
        }
        catch { }
        finally
        {
            MainThread.BeginInvokeOnMainThread(() => IsLoading = false);
        }
    }
}
