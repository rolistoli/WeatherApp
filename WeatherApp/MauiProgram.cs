using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
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
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<IGeocodingService, GeocodingService>();
        builder.Services.AddSingleton<INavigationService, AppNavigationService>();
        builder.Services.AddSingleton<IWeatherService, WeatherService>();
        
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<SearchPage>();

        
        return builder.Build();
    }
}