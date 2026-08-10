using BabyTracker.Data;

namespace BabyTracker.Tests;

public class UnitConverterTests
{
    [Fact]
    public void CelsiusToFahrenheit_KnownReferencePoints()
    {
        Assert.Equal(32, UnitConverter.CelsiusToFahrenheit(0));
        Assert.Equal(212, UnitConverter.CelsiusToFahrenheit(100));
    }

    [Fact]
    public void FahrenheitToCelsius_KnownReferencePoints()
    {
        Assert.Equal(0, UnitConverter.FahrenheitToCelsius(32));
        Assert.Equal(100, UnitConverter.FahrenheitToCelsius(212));
    }

    [Fact]
    public void KgToLb_ThenBack_RoundTripsWithinRoundingTolerance()
    {
        var original = 3.5m;
        var converted = UnitConverter.LbToKg(UnitConverter.KgToLb(original));
        Assert.True(Math.Abs(converted - original) < 0.0001m);
    }
}