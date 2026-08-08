namespace BabyTracker.App.Services;

// Single source of truth for "which child is currently active" — every tracker
// ViewModel reads ChildId from here instead of re-querying the database.
// When multi-child support is built later, this becomes the thing a "switch child"
// screen updates, and everything downstream keeps working unchanged.
public class CurrentChildContext
{
    private const string PreferenceKey = "current_child_id";

    public Guid? ChildId { get; private set; }
    public string? ChildName { get; private set; }

    // Fired whenever the active child changes, so any screen showing
    // child-specific data (Home, in the future: statistics) can refresh itself.
    public event Action? Changed;

    public void Set(Guid id, string name)
    {
        ChildId = id;
        ChildName = name;
        Preferences.Default.Set(PreferenceKey, id.ToString());
        Changed?.Invoke();
    }

    public static Guid? GetLastSelectedChildId()
    {
        var saved = Preferences.Default.Get(PreferenceKey, (string?)null);
        return Guid.TryParse(saved, out var id) ? id : null;
    }

    public void Clear()
    {
        ChildId = null;
        ChildName = null;
        Preferences.Default.Remove(PreferenceKey);
        Changed?.Invoke();
    }
}