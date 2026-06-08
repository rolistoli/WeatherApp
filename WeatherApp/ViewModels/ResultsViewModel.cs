using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Helpers;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.ViewModels;

public sealed partial class ResultsViewModel : BaseViewModel
{
    private readonly IWeatherService weatherService;
    private CityLocation? currentLocation;
    private WeatherForecast? currentForecast;
    private string locationName = string.Empty;
    private string currentTemperature = string.Empty;
    private string weatherDescription = string.Empty;
    private bool hasForecast;

    public ResultsViewModel(IWeatherService weatherService)
    {
        this.weatherService = weatherService;
    }

    public string LocationName
    {
        get => locationName;
        set => SetProperty(ref locationName, value);
    }

    public string CurrentTemperature
    {
        get => currentTemperature;
        set => SetProperty(ref currentTemperature, value);
    }

    public string WeatherDescription
    {
        get => weatherDescription;
        set => SetProperty(ref weatherDescription, value);
    }

    public bool HasForecast
    {
        get => hasForecast;
        set => SetProperty(ref hasForecast, value);
    }

    public ObservableCollection<DailyForecast> Forecast { get; } = [];

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
            var forecast = await weatherService.GetForecastAsync(currentLocation.Latitude, currentLocation.Longitude);
            currentForecast = forecast;
            ApplyForecast(forecast);
            HasForecast = true;
        }
        catch (HttpRequestException exception)
        {
            SetLocalizedError(exception.StatusCode is HttpStatusCode.BadRequest
                ? "ResultsBadRequest"
                : "WeatherConnectionError");
        }
        catch (TaskCanceledException)
        {
            SetLocalizedError("RequestTimeout");
        }
        catch (JsonException)
        {
            SetLocalizedError("WeatherInvalidApiData");
        }
        catch (InvalidOperationException exception)
        {
            ErrorMessage = exception.Message;
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

    protected override void RefreshLocalizedState()
    {
        base.RefreshLocalizedState();

        if (currentForecast is not null)
        {
            ApplyForecast(currentForecast);
        }
    }

    private void ApplyForecast(WeatherForecast forecast)
    {
        CurrentTemperature = $"{forecast.CurrentTemperature:F1} °C";
        WeatherDescription = WeatherCodeDescriptions.GetDescription(forecast.CurrentWeatherCode);

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
