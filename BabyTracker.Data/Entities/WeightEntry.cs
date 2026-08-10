namespace BabyTracker.Data;

public class WeightEntry : ChildScopedEntity
{
    public decimal WeightKg { get; set; }
    public string? Notes { get; set; }
}