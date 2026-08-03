namespace BabyTracker.Data;

// Every entity that can be synced between devices (i.e. almost everything in this app)
// needs these three fields for the merge logic we discussed: Id is generated on-device
// (so two phones can never collide), UpdatedAt drives last-write-wins conflict resolution,
// and DeletedAt is a soft-delete "tombstone" so a deletion can't be undone by a stale sync.
public abstract class SyncableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}