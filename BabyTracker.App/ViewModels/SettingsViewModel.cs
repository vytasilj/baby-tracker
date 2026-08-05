using CommunityToolkit.Mvvm.ComponentModel;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    public IReadOnlyList<SupportedLanguage> Languages { get; } = SupportedLanguages.All;

    [ObservableProperty]
    private SupportedLanguage _selectedLanguage;

    public SettingsViewModel()
    {
        _selectedLanguage = Languages.FirstOrDefault(l => l.Code == LocalizationResourceManager.Instance.CurrentLanguageCode)
            ?? Languages[0];
    }

    partial void OnSelectedLanguageChanged(SupportedLanguage value)
    {
        LocalizationResourceManager.Instance.SetLanguage(value.Code);
    }
}