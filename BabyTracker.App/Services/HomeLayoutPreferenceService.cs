namespace BabyTracker.App.Services;

public class HomeLayoutPreferenceService
{
    private const string PreferenceKey = "home_visible_trackers";
    private static readonly TrackerKind[] DefaultVisible =
        [TrackerKind.Feeding, TrackerKind.Sleep, TrackerKind.Diaper, TrackerKind.MomSleep];

    private readonly HashSet<TrackerKind> _visible;

    public HomeLayoutPreferenceService()
    {
        var saved = Preferences.Default.Get(PreferenceKey, string.Join(",", DefaultVisible));
        _visible = saved
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => Enum.TryParse<TrackerKind>(s, out var kind) ? (TrackerKind?)kind : null)
            .Where(k => k.HasValue)
            .Select(k => k!.Value)
            .ToHashSet();
    }

    public bool IsVisible(TrackerKind kind) => _visible.Contains(kind);

    public void SetVisible(TrackerKind kind, bool visible)
    {
        if (visible) _visible.Add(kind);
        else _visible.Remove(kind);
        Preferences.Default.Set(PreferenceKey, string.Join(",", _visible));
    }
}