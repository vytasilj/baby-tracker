namespace BabyTracker.Data;

public record DailySummary(int FeedingCount, double SleepHours, int DiaperCount);

public static class DailySummaryCalculator
{
    public static DailySummary Calculate(
        DateOnly day,
        IEnumerable<FeedingEntry> feedings,
        IEnumerable<SleepEntry> sleeps,
        IEnumerable<DiaperEntry> diapers,
        DateTime now)
    {
        var dayStart = day.ToDateTime(TimeOnly.MinValue);
        var dayEnd = dayStart.AddDays(1);

        var feedingCount = feedings.Count(f => f.OccurredAt >= dayStart && f.OccurredAt < dayEnd);
        var diaperCount = diapers.Count(d => d.OccurredAt >= dayStart && d.OccurredAt < dayEnd);

        var sleepHours = sleeps
            .Select(s => OverlapHours(s.StartTime, s.EndTime ?? now, dayStart, dayEnd))
            .Sum();

        return new DailySummary(feedingCount, Math.Round(sleepHours, 1), diaperCount);
    }

    // A sleep entry might start before midnight and end after it (or still be ongoing) —
    // this returns only the portion of the entry that actually falls within [dayStart, dayEnd),
    // so a night's sleep is correctly split across the two days it spans.
    private static double OverlapHours(DateTime start, DateTime end, DateTime dayStart, DateTime dayEnd)
    {
        var overlapStart = start > dayStart ? start : dayStart;
        var overlapEnd = end < dayEnd ? end : dayEnd;
        var overlap = overlapEnd - overlapStart;
        return overlap > TimeSpan.Zero ? overlap.TotalHours : 0;
    }
}