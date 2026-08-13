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

        var sleepHours = SleepHoursCalculator.TotalHoursForDay(day, sleeps.Select(s => (s.StartTime, s.EndTime)), now);

        return new DailySummary(feedingCount, Math.Round(sleepHours, 1), diaperCount);
    }
}