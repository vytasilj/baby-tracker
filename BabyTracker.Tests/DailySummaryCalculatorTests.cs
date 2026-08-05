using BabyTracker.Data;

namespace BabyTracker.Tests;

public class DailySummaryCalculatorTests
{
    private static readonly Guid ChildId = Guid.NewGuid();
    private static readonly DateOnly Today = new(2026, 8, 5);

    [Fact]
    public void Calculate_CountsFeedingsAndDiapersOnlyForGivenDay()
    {
        var feedings = new List<FeedingEntry>
        {
            new() { ChildId = ChildId, OccurredAt = new DateTime(2026, 8, 5, 8, 0, 0), Type = FeedingType.Bottle },
            new() { ChildId = ChildId, OccurredAt = new DateTime(2026, 8, 4, 23, 0, 0), Type = FeedingType.Bottle }, // previous day
        };
        var diapers = new List<DiaperEntry>
        {
            new() { ChildId = ChildId, OccurredAt = new DateTime(2026, 8, 5, 9, 0, 0), Type = DiaperType.Wet },
        };

        var result = DailySummaryCalculator.Calculate(Today, feedings, [], diapers, DateTime.Now);

        Assert.Equal(1, result.FeedingCount);
        Assert.Equal(1, result.DiaperCount);
    }

    [Fact]
    public void Calculate_SleepEntirelyWithinDay_CountsFullDuration()
    {
        var sleeps = new List<SleepEntry>
        {
            new() { ChildId = ChildId, StartTime = new DateTime(2026, 8, 5, 13, 0, 0), EndTime = new DateTime(2026, 8, 5, 15, 0, 0) }
        };

        var result = DailySummaryCalculator.Calculate(Today, [], sleeps, [], DateTime.Now);

        Assert.Equal(2.0, result.SleepHours);
    }

    [Fact]
    public void Calculate_SleepSpanningMidnight_SplitsAcrossBothDays()
    {
        var sleep = new SleepEntry { ChildId = ChildId, StartTime = new DateTime(2026, 8, 5, 22, 0, 0), EndTime = new DateTime(2026, 8, 6, 6, 0, 0) };

        var todayResult = DailySummaryCalculator.Calculate(Today, [], [sleep], [], DateTime.Now);
        var tomorrowResult = DailySummaryCalculator.Calculate(Today.AddDays(1), [], [sleep], [], DateTime.Now);

        Assert.Equal(2.0, todayResult.SleepHours);   // 22:00–24:00
        Assert.Equal(6.0, tomorrowResult.SleepHours); // 00:00–06:00
    }

    [Fact]
    public void Calculate_OngoingSleep_CountsUpToNow()
    {
        var now = new DateTime(2026, 8, 5, 14, 30, 0);
        var sleep = new SleepEntry { ChildId = ChildId, StartTime = new DateTime(2026, 8, 5, 13, 0, 0), EndTime = null };

        var result = DailySummaryCalculator.Calculate(Today, [], [sleep], [], now);

        Assert.Equal(1.5, result.SleepHours);
    }
}