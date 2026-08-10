namespace BabyTracker.Data;

public class TemperatureEntry : ChildScopedEntity
{
    public decimal ValueCelsius { get; set; }
    public string? Notes { get; set; }
}