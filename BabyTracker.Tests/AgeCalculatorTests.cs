using BabyTracker.Data;

namespace BabyTracker.Tests;

public class AgeCalculatorTests
{
    [Fact]
    public void Describe_NewbornSameDay_ReturnsZeroDaysAsOneDay()
    {
        var birth = new DateOnly(2026, 1, 1);
        Assert.Equal("1 day", AgeCalculator.Describe(birth, new DateOnly(2026, 1, 2)));
    }

    [Fact]
    public void Describe_UnderTwoWeeks_ReturnsDays()
    {
        var birth = new DateOnly(2026, 1, 1);
        Assert.Equal("10 days", AgeCalculator.Describe(birth, new DateOnly(2026, 1, 11)));
    }

    [Fact]
    public void Describe_UnderTwoMonths_ReturnsWeeksAndDays()
    {
        var birth = new DateOnly(2026, 1, 1);
        Assert.Equal("4w 3d", AgeCalculator.Describe(birth, new DateOnly(2026, 1, 1).AddDays(31)));
    }

    [Fact]
    public void Describe_OverTwoMonths_ReturnsMonthsAndDays()
    {
        var birth = new DateOnly(2026, 1, 15);
        Assert.Equal("3 months", AgeCalculator.Describe(birth, new DateOnly(2026, 4, 15)));
    }

    [Fact]
    public void MonthsAndDays_HandlesMonthBoundaryCorrectly()
    {
        var (months, days) = AgeCalculator.MonthsAndDays(new DateOnly(2026, 1, 31), new DateOnly(2026, 3, 1));
        Assert.Equal(1, months);
        Assert.Equal(1, days);
    }
}