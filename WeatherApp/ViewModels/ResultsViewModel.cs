using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using WeatherApp.Helpers;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.ViewModels;

public sealed partial class ResultsViewModel : BaseViewModel
{
    private readonly IWeatherService weatherService;
    private CityLocation? currentLocation;

    public ResultsViewModel(IWeatherService weatherService)
    {
        this.weatherService = weatherService;
    }

    [ObservableProperty]
    private string locationName = string.Empty;

    [ObservableProperty]
    private string currentTemperature = string.Empty;

    [ObservableProperty]
    private string weatherDescription = string.Empty;

    [ObservableProperty]
    private string currentDay = string.Empty;

    [ObservableProperty]
    private bool hasForecast;

    [ObservableProperty]
    private string currentBackgroundImage = string.Empty;

    public ObservableCollection<DailyForecast> Forecast { get; } = [];
    public ObservableCollection<HourlyForecast> Hourly { get; } = new();

    public async Task LoadAsync(CityLocation location)
    {
        currentLocation = location;
        LocationName = BuildLocationName(location);
        RetryCommand.NotifyCanExecuteChanged();
        await LoadWeatherAsync();
    }

    [RelayCommand(CanExecute = nameof(CanRetry))]
    private async Task RetryAsync()
    {
        if (currentLocation is not null)
        {
            await LoadWeatherAsync();
        }
    }

    private async Task LoadWeatherAsync()
    {
        if (currentLocation is null)
        {
            return;
        }

        ClearError();
        HasForecast = false;
        Forecast.Clear();
        IsLoading = true;
        RetryCommand.NotifyCanExecuteChanged();

        try
        {
            var forecast = await weatherService.GetForecastAsync(
                currentLocation.Latitude,
                currentLocation.Longitude);

            ApplyForecast(forecast);
            HasForecast = true;
        }      
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
            RetryCommand.NotifyCanExecuteChanged();
        }
    }

    private bool CanRetry()
    {
        return IsNotLoading && currentLocation is not null;
    }


    private void ApplyForecast(WeatherForecast forecast)
    {
        CurrentTemperature = $"{forecast.CurrentTemperature:F1} °C";
        WeatherDescription = WeatherCodeDescriptions.GetDescription(forecast.CurrentWeatherCode);
        CurrentBackgroundImage = forecast.CurrentBackgroundImage;

        // set current day from first daily forecast if available
        if (forecast.DailyForecasts != null && forecast.DailyForecasts.Count > 0)
        {
            CurrentDay = FormatDate(forecast.DailyForecasts[0].DateValue);
        }

        Hourly.Clear();

        Forecast.Clear();

        foreach (var dailyForecast in forecast.DailyForecasts)
        {
            Forecast.Add(new DailyForecast
            {
                Date = FormatDate(dailyForecast.DateValue),
                DateValue = dailyForecast.DateValue,
                WeatherCode = dailyForecast.WeatherCode,
                TemperatureMin = dailyForecast.TemperatureMin,
                TemperatureMax = dailyForecast.TemperatureMax,
                Description = WeatherCodeDescriptions.GetDescription(dailyForecast.WeatherCode),
                TemperatureRange = $"{dailyForecast.TemperatureMin:F0}°C / {dailyForecast.TemperatureMax:F0}°C"
            });
        }

        foreach (var hourly in forecast.HourlyForecasts)
        {
            Hourly.Add(hourly);
        }
    }

    private static string BuildLocationName(CityLocation location)
    {
        var parts = new[] { location.Name, location.Admin1, location.Country }
            .Where(part => !string.IsNullOrWhiteSpace(part));

        return string.Join(", ", parts);
    }

    private static string FormatDate(string value)
    {
        return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var date)
            ? date.ToString("ddd, dd MMM", CultureInfo.CurrentCulture)
            : value;
    }
}
