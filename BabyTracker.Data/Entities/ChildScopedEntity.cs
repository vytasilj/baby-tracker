namespace BabyTracker.Data;

// Shared by every tracker that: belongs to one child, happens at one point in time,
// and has optional notes. (Sleep doesn't fit — it has a start AND an optional end —
// so it keeps its own SleepRepository, unchanged.)
public abstract class ChildScopedEntity : SyncableEntity
{
    public Guid ChildId { get; set; }
    public DateTime OccurredAt { get; set; }
}