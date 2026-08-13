namespace BabyTracker.Data;

public static class SleepHoursCalculator
{
    public static double TotalHoursForDay(DateOnly day, IEnumerable<(DateTime Start, DateTime? End)> sleeps, DateTime now)
    {
        var dayStart = day.ToDateTime(TimeOnly.MinValue);
        var dayEnd = dayStart.AddDays(1);
        return sleeps.Sum(s => OverlapHours(s.Start, s.End ?? now, dayStart, dayEnd));
    }

    private static double OverlapHours(DateTime start, DateTime end, DateTime dayStart, DateTime dayEnd)
    {
        var overlapStart = start > dayStart ? start : dayStart;
        var overlapEnd = end < dayEnd ? end : dayEnd;
        var overlap = overlapEnd - overlapStart;
        return overlap > TimeSpan.Zero ? overlap.TotalHours : 0;
    }
}