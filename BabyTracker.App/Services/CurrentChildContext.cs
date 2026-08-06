namespace BabyTracker.App.Services;

// Single source of truth for "which child is currently active" — every tracker
// ViewModel reads ChildId from here instead of re-querying the database.
// When multi-child support is built later, this becomes the thing a "switch child"
// screen updates, and everything downstream keeps working unchanged.
public class CurrentChildContext
{
    public Guid? ChildId { get; private set; }
    public string? ChildName { get; private set; }

    public void Set(Guid id, string name)
    {
        ChildId = id;
        ChildName = name;
    }
}