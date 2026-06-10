using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
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
        try
        {
            var navigation = Application.Current?.Windows.FirstOrDefault()?.Page?.Navigation;
            if (navigation is not null && navigation.ModalStack.Count > 0)
            {

                await navigation.PopModalAsync(false);
                return;
            }
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex);
        }
    }

    [RelayCommand]
    private async Task Details()
    {
        if (Location is null)
        {
            return;
        }

        try
        {
            IsLoading = true;

            await navigationService.GoToResultsAsync(Location);
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex);
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
        IsLoading = true;

        Location = loc;
        Title = string.Empty;
        Subtitle = string.Empty;
        Temperature = string.Empty;
        Description = string.Empty;
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

            var pw = await weatherService.GetPointWeatherAsync(
                loc.Latitude,
                loc.Longitude);

            Temperature = $"{pw.Temperature:F1} °C";
            Description = pw.Description;
            WeatherImage = WeatherBackground.GetBackgroundForCode(pw.WeatherCode);
        }
        catch (Exception ex)
        {
            await ShowErrorAsync(ex);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
