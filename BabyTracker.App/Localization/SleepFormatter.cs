namespace BabyTracker.App.Localization;

public static class SleepFormatter
{
    public static string FormatDuration(DateTime start, DateTime? end)
    {
        if (end is null)
        {
            return LocalizationResourceManager.Instance["Common_InProgress"];
        }

        var duration = end.Value - start;
        var hours = (int)duration.TotalHours;
        var minutes = duration.Minutes;

        if (hours == 0) return PluralFormatter.Format(minutes, "Minute");
        if (minutes == 0) return PluralFormatter.Format(hours, "Hour");
        return $"{PluralFormatter.Format(hours, "Hour")} {PluralFormatter.Format(minutes, "Minute")}";
    }
}