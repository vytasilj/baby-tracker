namespace BabyTracker.App.Localization;

public static class PluralFormatter
{
    public static string Format(int count, string unitKey)
    {
        var loc = LocalizationResourceManager.Instance;
        var category = BabyTracker.Data.PluralRules.GetCategory(loc.CurrentLanguageCode, count);
        return $"{count} {loc[$"Unit_{unitKey}_{category}"]}";
    }
}