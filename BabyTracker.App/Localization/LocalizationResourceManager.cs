using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace BabyTracker.App.Localization;

public class LocalizationResourceManager : INotifyPropertyChanged
{
    private const string PreferenceKey = "app_language";
    private const string DefaultLanguage = "en";

    public static LocalizationResourceManager Instance { get; } = new();

    private readonly ResourceManager _resourceManager =
        new("BabyTracker.App.Resources.Strings.AppResources", typeof(LocalizationResourceManager).Assembly);

    private CultureInfo _currentCulture = new(DefaultLanguage);

    private LocalizationResourceManager() { }

    public string CurrentLanguageCode => _currentCulture.TwoLetterISOLanguageName;

    public string this[string key] => _resourceManager.GetString(key, _currentCulture) ?? key;

    public CultureInfo NumberFormatCulture
    {
        get
        {
            try { return new CultureInfo(CurrentLanguageCode); }
            catch (CultureNotFoundException) { return CultureInfo.InvariantCulture; }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    // Called once at startup, mirrors ThemeService.ApplySavedTheme().
    public void ApplySavedLanguage()
    {
        var saved = Preferences.Default.Get(PreferenceKey, (string?)null);
        var languageCode = saved ?? DetectSystemLanguage();
        SetLanguage(languageCode, persist: false);
    }

    private static string DetectSystemLanguage()
    {
        var systemCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        return SupportedLanguages.IsSupported(systemCode) ? systemCode : DefaultLanguage;
    }

    public void SetLanguage(string languageCode) => SetLanguage(languageCode, persist: true);

    private void SetLanguage(string languageCode, bool persist)
    {
        _currentCulture = new CultureInfo(languageCode);

        if (persist)
        {
            Preferences.Default.Set(PreferenceKey, languageCode);
        }

        // "Item[]" is the standard .NET convention meaning "this indexer's values changed" —
        // it's what makes every {loc:Translate} binding across the whole app refresh instantly.
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }
}