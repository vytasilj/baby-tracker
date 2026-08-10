using System.Globalization;
using BabyTracker.Data;

namespace BabyTracker.Tests;

public class WeightFormatterTests
{
    [Fact]
    public void FormatForDisplay_Metric_InvariantCulture_UsesDot()
    {
        Assert.Equal("3.500 kg", WeightFormatter.FormatForDisplay(3.5m, UnitSystem.Metric));
    }

    [Fact]
    public void FormatForDisplay_Metric_CzechCulture_UsesComma()
    {
        var result = WeightFormatter.FormatForDisplay(3.5m, UnitSystem.Metric, new CultureInfo("cs-CZ"));
        Assert.Equal("3,500 kg", result);
    }

    [Fact]
    public void FormatForDisplay_Imperial_ConvertsAndShowsPounds()
    {
        Assert.Equal("2.205 lb", WeightFormatter.FormatForDisplay(1m, UnitSystem.Imperial));
    }

    [Fact]
    public void ToCanonicalKg_Imperial_ThenToDisplayValue_RoundTrips()
    {
        var canonical = WeightFormatter.ToCanonicalKg(10, UnitSystem.Imperial);
        var backToDisplay = WeightFormatter.ToDisplayValue(canonical, UnitSystem.Imperial);
        Assert.True(Math.Abs(backToDisplay - 10) < 0.001);
    }
}