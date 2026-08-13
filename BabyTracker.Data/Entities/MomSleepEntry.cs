namespace BabyTracker.Data;

// Deliberately NOT a ChildScopedEntity — mom's sleep is one shared thing for the
// whole family, independent of which child (or how many children) exist.
public class MomSleepEntry : SyncableEntity
{
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Notes { get; set; }
}