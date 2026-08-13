using BabyTracker.Data;

namespace BabyTracker.Tests;

public class SleepHoursCalculatorTests
{
    [Fact]
    public void TotalHoursForDay_SleepSpanningMidnight_SplitsAcrossBothDays()
    {
        var sleeps = new List<(DateTime, DateTime?)>
        {
            (new DateTime(2026, 8, 5, 22, 0, 0), new DateTime(2026, 8, 6, 6, 0, 0))
        };

        var today = SleepHoursCalculator.TotalHoursForDay(new DateOnly(2026, 8, 5), sleeps, DateTime.Now);
        var tomorrow = SleepHoursCalculator.TotalHoursForDay(new DateOnly(2026, 8, 6), sleeps, DateTime.Now);

        Assert.Equal(2.0, today);
        Assert.Equal(6.0, tomorrow);
    }

    [Fact]
    public void TotalHoursForDay_OngoingSleep_CountsUpToNow()
    {
        var now = new DateTime(2026, 8, 5, 14, 30, 0);
        var sleeps = new List<(DateTime, DateTime?)> { (new DateTime(2026, 8, 5, 13, 0, 0), null) };

        var result = SleepHoursCalculator.TotalHoursForDay(new DateOnly(2026, 8, 5), sleeps, now);

        Assert.Equal(1.5, result);
    }
}