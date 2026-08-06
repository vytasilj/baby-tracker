using BabyTracker.Data;

namespace BabyTracker.App.Localization;

public static class AgeFormatter
{
    public static string Format(AgeDescription age)
    {
        var loc = LocalizationResourceManager.Instance;

        return age.Unit switch
        {
            AgeUnit.NotBornYet => loc["Age_NotBornYet"],
            AgeUnit.Days => FormatUnit(age.Primary, "Day"),
            AgeUnit.Weeks when age.Secondary == 0 => FormatUnit(age.Primary, "Week"),
            AgeUnit.Weeks => $"{FormatUnit(age.Primary, "Week")} {FormatUnit(age.Secondary, "Day")}",
            AgeUnit.Months when age.Secondary == 0 => FormatUnit(age.Primary, "Month"),
            AgeUnit.Months => $"{FormatUnit(age.Primary, "Month")} {FormatUnit(age.Secondary, "Day")}",
            _ => ""
        };
    }

    private static string FormatUnit(int count, string unitKey) => PluralFormatter.Format(count, unitKey);
}