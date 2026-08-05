namespace BabyTracker.App.Localization;

public record SupportedLanguage(string Code, string DisplayName);

public static class SupportedLanguages
{
    // Single source of truth: adding a language means one new line here,
    // plus the matching AppResources.<code>.resx file. Nothing else changes.
    public static IReadOnlyList<SupportedLanguage> All { get; } =
    [
        new("en", "English"),
        new("cs", "Čeština"),
    ];

    public static bool IsSupported(string code) => All.Any(l => l.Code == code);
}