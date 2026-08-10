using System.Globalization;
using BabyTracker.Data;

namespace BabyTracker.Tests;

public class TemperatureFormatterTests
{
    [Fact]
    public void FormatForDisplay_Metric_InvariantCulture_UsesDot()
    {
        Assert.Equal("36.6 °C", TemperatureFormatter.FormatForDisplay(36.6m, UnitSystem.Metric));
    }

    [Fact]
    public void FormatForDisplay_Metric_CzechCulture_UsesComma()
    {
        var result = TemperatureFormatter.FormatForDisplay(36.6m, UnitSystem.Metric, new CultureInfo("cs-CZ"));
        Assert.Equal("36,6 °C", result);
    }

    [Fact]
    public void FormatForDisplay_Imperial_ConvertsAndShowsFahrenheit()
    {
        Assert.Equal("98.1 °F", TemperatureFormatter.FormatForDisplay(36.7m, UnitSystem.Imperial));
    }

    [Fact]
    public void StepperRange_Imperial_CoversTypicalHumanRange()
    {
        Assert.Equal(86, TemperatureFormatter.StepperMinimum(UnitSystem.Imperial));
        Assert.Equal(109.4, TemperatureFormatter.StepperMaximum(UnitSystem.Imperial));
    }
}