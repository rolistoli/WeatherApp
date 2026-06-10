using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using WeatherApp.Services;
using WeatherApp.Services.Interfaces;
using WeatherApp.ViewModels;
using WeatherApp.Views;

namespace WeatherApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton(sp =>
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("WeatherApp/1.0");
            client.DefaultRequestHeaders.From = "jmiguelroliveira@gmail.com";
            return client;
        });
        builder.Services.AddSingleton<IGeocodingService, GeocodingService>();
        builder.Services.AddSingleton<INavigationService, AppNavigationService>();
        builder.Services.AddSingleton<IWeatherService, WeatherService>();

        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<SearchPage>();       
        builder.Services.AddTransient<ResultsViewModel>();
        builder.Services.AddTransient<ResultsPage>();
        builder.Services.AddTransient<LocationPopupViewModel>();
        builder.Services.AddTransient<LocationPopup>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<SettingsPageViewModel>();

        return builder.Build();
    }
}