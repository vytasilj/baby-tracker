using BabyTracker.Data;

namespace BabyTracker.Tests;

public class AgeCalculatorTests
{
    [Fact]
    public void Calculate_NewbornSameDay_ReturnsZeroDaysAsOneDay()
    {
        var birth = new DateOnly(2026, 1, 1);
        var result = AgeCalculator.Calculate(birth, new DateOnly(2026, 1, 2));
        Assert.Equal(AgeUnit.Days, result.Unit);
        Assert.Equal(1, result.Primary);
    }

    [Fact]
    public void Calculate_UnderTwoWeeks_ReturnsDays()
    {
        var birth = new DateOnly(2026, 1, 1);
        var result = AgeCalculator.Calculate(birth, new DateOnly(2026, 1, 11));
        Assert.Equal(AgeUnit.Days, result.Unit);
        Assert.Equal(10, result.Primary);
    }

    [Fact]
    public void Calculate_UnderTwoMonths_ReturnsWeeksAndDays()
    {
        var birth = new DateOnly(2026, 1, 1);
        var result = AgeCalculator.Calculate(birth, new DateOnly(2026, 1, 1).AddDays(31));
        Assert.Equal(AgeUnit.Weeks, result.Unit);
        Assert.Equal(4, result.Primary);
        Assert.Equal(3, result.Secondary);
    }

    [Fact]
    public void Calculate_OverTwoMonths_ReturnsMonthsAndDays()
    {
        var birth = new DateOnly(2026, 1, 15);
        var result = AgeCalculator.Calculate(birth, new DateOnly(2026, 4, 15));
        Assert.Equal(AgeUnit.Months, result.Unit);
        Assert.Equal(3, result.Primary);
    }

    [Fact]
    public void Calculate_Exactly13Days_StillReturnsDays()
    {
        var birth = new DateOnly(2026, 1, 1);
        var result = AgeCalculator.Calculate(birth, birth.AddDays(13));
        Assert.Equal(AgeUnit.Days, result.Unit);
    }

    [Fact]
    public void Calculate_Exactly14Days_SwitchesToWeeks()
    {
        var birth = new DateOnly(2026, 1, 1);
        var result = AgeCalculator.Calculate(birth, birth.AddDays(14));
        Assert.Equal(AgeUnit.Weeks, result.Unit);
        Assert.Equal(2, result.Primary);
        Assert.Equal(0, result.Secondary);
    }

    [Fact]
    public void Calculate_Exactly59Days_StillReturnsWeeks()
    {
        var birth = new DateOnly(2026, 1, 1);
        var result = AgeCalculator.Calculate(birth, birth.AddDays(59));
        Assert.Equal(AgeUnit.Weeks, result.Unit);
    }

    [Fact]
    public void Calculate_Exactly60Days_SwitchesToMonths()
    {
        var birth = new DateOnly(2026, 1, 1);
        var result = AgeCalculator.Calculate(birth, birth.AddDays(60));
        Assert.Equal(AgeUnit.Months, result.Unit);
    }

    [Fact]
    public void Calculate_FutureBirthDate_ReturnsNotBornYet()
    {
        var birth = new DateOnly(2026, 12, 1);
        var result = AgeCalculator.Calculate(birth, new DateOnly(2026, 8, 5));
        Assert.Equal(AgeUnit.NotBornYet, result.Unit);
    }

    [Fact]
    public void MonthsAndDays_HandlesMonthBoundaryCorrectly()
    {
        var (months, days) = AgeCalculator.MonthsAndDays(new DateOnly(2026, 1, 31), new DateOnly(2026, 3, 1));
        Assert.Equal(1, months);
        Assert.Equal(1, days);
    }
}