namespace BabyTracker.App.Services;
using BabyTracker.Data;
public class UnitPreferenceService
{
    private const string PreferenceKey = "unit_system";

    public UnitSystem Current { get; private set; }

    public UnitPreferenceService()
    {
        var saved = Preferences.Default.Get(PreferenceKey, nameof(UnitSystem.Metric));
        Current = Enum.TryParse<UnitSystem>(saved, out var parsed) ? parsed : UnitSystem.Metric;
    }

    public void Set(UnitSystem system)
    {
        Current = system;
        Preferences.Default.Set(PreferenceKey, system.ToString());
    }
}