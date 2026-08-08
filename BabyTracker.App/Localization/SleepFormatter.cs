namespace BabyTracker.App.Localization;

public static class SleepFormatter
{
    public static string FormatDuration(DateTime start, DateTime? end)
    {
        if (end is null)
        {
            return LocalizationResourceManager.Instance["Common_InProgress"];
        }

        return FormatTotalHours((end.Value - start).TotalHours);
    }

    public static string FormatTotalHours(double totalHours)
    {
        var totalMinutes = (int)Math.Round(totalHours * 60);
        var hours = totalMinutes / 60;
        var minutes = totalMinutes % 60;

        if (hours == 0) return PluralFormatter.Format(minutes, "Minute");
        if (minutes == 0) return PluralFormatter.Format(hours, "Hour");
        return $"{PluralFormatter.Format(hours, "Hour")} {PluralFormatter.Format(minutes, "Minute")}";
    }
}