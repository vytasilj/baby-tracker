namespace BabyTracker.Data;

public class PumpingEntry : ChildScopedEntity
{
    public BreastSide Side { get; set; }
    public int? AmountMl { get; set; }
    public string? Notes { get; set; }
}