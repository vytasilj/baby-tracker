namespace BabyTracker.Data;

public class Child : SyncableEntity
{
    public required string Name { get; set; }
    public DateOnly BirthDate { get; set; }
}