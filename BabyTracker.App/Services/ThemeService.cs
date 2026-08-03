namespace BabyTracker.App.Services;

public static class ThemeService
{
    private const string PreferenceKey = "app_theme";

    // Called once at startup, before any page renders, to restore the user's last choice.
    public static void ApplySavedTheme()
    {
        var saved = Preferences.Default.Get(PreferenceKey, "Unspecified");
        Application.Current!.UserAppTheme = saved switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified // follow the phone's system setting until the user picks explicitly
        };
    }

    public static void ToggleTheme()
    {
        // RequestedTheme reflects what's actually showing right now (resolves "Unspecified" to the real OS theme)
        var effective = Application.Current!.RequestedTheme;
        var newTheme = effective == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;

        Application.Current.UserAppTheme = newTheme;
        Preferences.Default.Set(PreferenceKey, newTheme.ToString());
    }

    public static string GetIconForCurrentTheme() => Application.Current!.RequestedTheme == AppTheme.Dark ? "☀️" : "🌙";
}