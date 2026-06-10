using System.Globalization;
using WeatherApp.Views;
using WeatherApp.Helpers;

namespace WeatherApp;

public partial class App : Application
{
	private readonly IServiceProvider _serviceProvider;

	public App(IServiceProvider serviceProvider)
	{
		SetEnglishCulture();

		InitializeComponent();

		AppSettings.ApplySavedPreferences();

		_serviceProvider = serviceProvider;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var mainPage = _serviceProvider.GetRequiredService<MainPage>();

		return new Window(new NavigationPage(mainPage));
	}

	private static void SetEnglishCulture()
	{
		var culture = CultureInfo.GetCultureInfo("en-US");

		CultureInfo.DefaultThreadCurrentCulture = culture;
		CultureInfo.DefaultThreadCurrentUICulture = culture;
		CultureInfo.CurrentCulture = culture;
		CultureInfo.CurrentUICulture = culture;
	}
}
