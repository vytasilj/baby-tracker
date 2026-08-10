using CommunityToolkit.Mvvm.ComponentModel;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;
using BabyTracker.Data;

namespace BabyTracker.App.ViewModels;

public record UnitSystemOption(UnitSystem Value, string Display);

public partial class SettingsViewModel : ObservableObject
{
    private readonly UnitPreferenceService _unitPreference;

    public IReadOnlyList<SupportedLanguage> Languages { get; } = SupportedLanguages.All;
    public IReadOnlyList<UnitSystemOption> UnitSystems { get; }

    [ObservableProperty] private SupportedLanguage _selectedLanguage;
    [ObservableProperty] private UnitSystemOption _selectedUnitSystem;

    public SettingsViewModel(UnitPreferenceService unitPreference)
    {
        _unitPreference = unitPreference;

        _selectedLanguage = Languages.FirstOrDefault(l => l.Code == LocalizationResourceManager.Instance.CurrentLanguageCode)
            ?? Languages[0];

        var loc = LocalizationResourceManager.Instance;
        UnitSystems =
        [
            new(UnitSystem.Metric, loc["Settings_Unit_Metric"]),
            new(UnitSystem.Imperial, loc["Settings_Unit_Imperial"]),
        ];
        _selectedUnitSystem = UnitSystems.First(u => u.Value == unitPreference.Current);
    }

    partial void OnSelectedLanguageChanged(SupportedLanguage value)
    {
        LocalizationResourceManager.Instance.SetLanguage(value.Code);
    }

    partial void OnSelectedUnitSystemChanged(UnitSystemOption value) => _unitPreference.Set(value.Value);
}