using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
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
    private string currentBackgroundImage = string.Empty;

    public ObservableCollection<DailyForecast> Forecast { get; } = [];
    public ObservableCollection<HourlyForecast> Hourly { get; } = [];

    public async Task LoadAsync(CityLocation location)
    {
        currentLocation = location;
        LocationName = WeatherHelper.BuildLocationName(location);

        await LoadWeatherAsync();
    }

    private async Task LoadWeatherAsync()
    {
        Forecast.Clear();
        Hourly.Clear();

        if (currentLocation is null)
        {
            return;
        }

        IsLoading = true;

        try
        {
            var forecast = await weatherService.GetForecastAsync(
                currentLocation.Latitude,
                currentLocation.Longitude);

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

            if (forecast.HourlyForecasts is not null && forecast.HourlyForecasts.Count > 0)
            {
                foreach (var hourly in forecast.HourlyForecasts)
                {
                    Hourly.Add(hourly);
                }
            }           
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
