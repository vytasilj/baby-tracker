using System.Globalization;

namespace BabyTracker.Data;

public static class WeightFormatter
{
    public static string UnitLabel(UnitSystem system) => system == UnitSystem.Metric ? "kg" : "lb";

    public static double ToDisplayValue(decimal kg, UnitSystem system) =>
        (double)(system == UnitSystem.Metric ? kg : UnitConverter.KgToLb(kg));

    public static decimal ToCanonicalKg(double displayValue, UnitSystem system)
    {
        var value = (decimal)displayValue;
        return system == UnitSystem.Metric ? value : UnitConverter.LbToKg(value);
    }

    public static string FormatForDisplay(decimal kg, UnitSystem system, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.InvariantCulture;
        return $"{ToDisplayValue(kg, system).ToString("0.000", culture)} {UnitLabel(system)}";
    }
}