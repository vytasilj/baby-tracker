namespace BabyTracker.Data;

public enum DiaperType { Wet, Dirty, Both }

public class DiaperEntry : ChildScopedEntity
{
    public DiaperType Type { get; set; }
    public string? Notes { get; set; }
}