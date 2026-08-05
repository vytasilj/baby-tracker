namespace BabyTracker.Data;

public enum DiaperType { Wet, Dirty, Both }

public class DiaperEntry : SyncableEntity
{
    public Guid ChildId { get; set; }
    public DateTime OccurredAt { get; set; }
    public DiaperType Type { get; set; }
    public string? Notes { get; set; }
}