using BabyTracker.Data;

namespace BabyTracker.App.Localization;

public static class VaccineFormatter
{
    public static string DisplayName(VaccineDefinition definition) =>
        definition.BuiltInKey is { } key
            ? LocalizationResourceManager.Instance[$"Vaccine_{key}"]
            : definition.Name ?? "";
}