namespace BabyTracker.Data;

public class CalendarEvent : SyncableEntity
{
    // Null = family-wide event (e.g. mom's own appointment), not tied to any child.
    public Guid? ChildId { get; set; }
    public required string Title { get; set; }
    public DateTime OccursAt { get; set; }
    public string? Notes { get; set; }
}