namespace BabyTracker.Data;

// Extensible by design: adding a new tracker type in a later phase means adding
// a new TrackerKey value, not a new database column or migration.
public static class TrackerKeys
{
    public const string Feeding = "Feeding";
    public const string Sleep = "Sleep";
    public const string Diaper = "Diaper";
}

public class TrackerSetting : SyncableEntity
{
    public Guid ChildId { get; set; }
    public required string TrackerKey { get; set; }
    public bool IsEnabled { get; set; } = true;
}