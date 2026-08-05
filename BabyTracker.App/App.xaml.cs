using BabyTracker.App.Views;

namespace BabyTracker.App;

public partial class App : Application
{
	private readonly StartupPage _startupPage;

	public App(StartupPage startupPage)
	{
		InitializeComponent();
		Services.ThemeService.ApplySavedTheme();
		Localization.LocalizationResourceManager.Instance.ApplySavedLanguage();
		_startupPage = startupPage;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new NavigationPage(_startupPage));
	}
}