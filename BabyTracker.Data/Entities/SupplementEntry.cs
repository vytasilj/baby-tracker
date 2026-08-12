namespace BabyTracker.Data;

public class SupplementEntry : ChildScopedEntity
{
    public string? Notes { get; set; }
    public List<SupplementDefinition> Supplements { get; set; } = [];
}