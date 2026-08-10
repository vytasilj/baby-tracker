namespace BabyTracker.Data;

public static class UnitConverter
{
    private const decimal KgToLbFactor = 2.2046226218m;

    public static decimal KgToLb(decimal kg) => kg * KgToLbFactor;
    public static decimal LbToKg(decimal lb) => lb / KgToLbFactor;

    public static decimal CelsiusToFahrenheit(decimal celsius) => celsius * 9 / 5 + 32;
    public static decimal FahrenheitToCelsius(decimal fahrenheit) => (fahrenheit - 32) * 5 / 9;
}