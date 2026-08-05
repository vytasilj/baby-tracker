namespace BabyTracker.Data;

// Maps a count to a grammatical category name, per language. Most languages
// (English, German, ...) only distinguish "one" vs "everything else" — that's
// the default. Languages with richer plural grammar get their own rule here;
// adding a new language does NOT require touching this file unless its grammar
// needs more than the default one/other split.
public static class PluralRules
{
    private static readonly Dictionary<string, Func<int, string>> LanguageSpecificRules = new()
    {
        ["cs"] = CzechRule,
    };

    public static string GetCategory(string languageCode, int count) =>
        LanguageSpecificRules.TryGetValue(languageCode, out var rule) ? rule(count) : DefaultRule(count);

    private static string DefaultRule(int count) => Math.Abs(count) == 1 ? "One" : "Other";

    private static string CzechRule(int count)
    {
        var abs = Math.Abs(count);
        if (abs == 1) return "One";
        if (abs is >= 2 and <= 4) return "Few";
        return "Other";
    }
}