using BabyTracker.Data;

namespace BabyTracker.App.Localization;

public static class SupplementFormatter
{
    public static string DisplayName(SupplementDefinition definition) =>
        definition.BuiltInKey is { } key
            ? LocalizationResourceManager.Instance[$"Supplement_{key}"]
            : definition.Name ?? "";
}