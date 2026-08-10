using System.Globalization;

namespace BabyTracker.Data;

public static class TemperatureFormatter
{
    public static string UnitLabel(UnitSystem system) => system == UnitSystem.Metric ? "°C" : "°F";

    public static double ToDisplayValue(decimal celsius, UnitSystem system) =>
        (double)(system == UnitSystem.Metric ? celsius : UnitConverter.CelsiusToFahrenheit(celsius));

    public static decimal ToCanonicalCelsius(double displayValue, UnitSystem system)
    {
        var value = (decimal)displayValue;
        return system == UnitSystem.Metric ? value : UnitConverter.FahrenheitToCelsius(value);
    }

    public static string FormatForDisplay(decimal celsius, UnitSystem system, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.InvariantCulture;
        return $"{ToDisplayValue(celsius, system).ToString("0.0", culture)} {UnitLabel(system)}";
    }

    public static double StepperMinimum(UnitSystem system) => system == UnitSystem.Metric ? 30 : 86;
    public static double StepperMaximum(UnitSystem system) => system == UnitSystem.Metric ? 43 : 109.4;
    public static double StepperIncrement(UnitSystem system) => system == UnitSystem.Metric ? 0.1 : 0.2;
}