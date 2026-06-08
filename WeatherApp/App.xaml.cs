using WeatherApp.Views;

namespace WeatherApp;

public partial class App : Application
{
	private readonly IServiceProvider _serviceProvider;

	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		// AppSettings.ApplySavedPreferences();
		_serviceProvider = serviceProvider;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var searchPage = _serviceProvider.GetRequiredService<SearchPage>();

		return new Window(new NavigationPage(searchPage));	}
}