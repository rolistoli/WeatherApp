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
        LocationName = WeatherHelper.BuildLocationName(location);
        await LoadWeatherAsync();
    } 

    private async Task LoadWeatherAsync()
    {
        if (currentLocation is null)
        {
            return;
        }

        HasForecast = false;
        Forecast.Clear();
        IsLoading = true;

        try
        {
            var forecast = await weatherService.GetForecastAsync(
                currentLocation.Latitude,
                currentLocation.Longitude);

            ApplyForecast(forecast);
            HasForecast = true;
        }      
        catch
        {
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ApplyForecast(WeatherForecast forecast)
    {
        Hourly.Clear();
        Forecast.Clear();

        if (forecast is null)
        { 
            return;
        }

        CurrentTemperature = $"{forecast.CurrentTemperature:F1} °C";
        WeatherDescription = WeatherCodeDescriptions.GetDescription(forecast.CurrentWeatherCode);
        CurrentBackgroundImage = forecast.CurrentBackgroundImage;

        if (forecast.DailyForecasts is not null && forecast.DailyForecasts.Count > 0)
        {
            CurrentDay = WeatherHelper.FormatDate(forecast.DailyForecasts[0].DateValue);

            foreach (var dailyForecast in forecast.DailyForecasts)
            {
                Forecast.Add(new DailyForecast
                {
                Date = WeatherHelper.FormatDate(dailyForecast.DateValue),
                    DateValue = dailyForecast.DateValue,
                    WeatherCode = dailyForecast.WeatherCode,
                    TemperatureMin = dailyForecast.TemperatureMin,
                    TemperatureMax = dailyForecast.TemperatureMax,
                    Description = WeatherCodeDescriptions.GetDescription(dailyForecast.WeatherCode),
                    TemperatureRange = $"{dailyForecast.TemperatureMin:F0}°C / {dailyForecast.TemperatureMax:F0}°C"
                });
            }
        }

        foreach (var hourly in forecast.HourlyForecasts)
        {
            Hourly.Add(hourly);
        }
    }
}
