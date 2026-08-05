namespace BabyTracker.Data;

public class SleepEntry : SyncableEntity
{
    public Guid ChildId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Notes { get; set; }
}