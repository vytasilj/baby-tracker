namespace BabyTracker.Data;

public enum FeedingType { Breast, Bottle, Solid }
public enum BreastSide { Left, Right, Both }

public class FeedingEntry : ChildScopedEntity
{
    public FeedingType Type { get; set; }
    public BreastSide? Side { get; set; }
    public int? AmountMl { get; set; }
    public string? Notes { get; set; }
}