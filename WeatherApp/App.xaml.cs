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

	// Expose the application's service provider so views can resolve services when
	// they are created by the XAML loader and can't receive constructor injection.
	public IServiceProvider Services => _serviceProvider;

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var searchPage = _serviceProvider.GetRequiredService<SearchPage>();

		return new Window(new NavigationPage(searchPage));	}
}