using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class TrackerVisibilityItem : ObservableObject
{
    public TrackerKind Kind { get; }
    public string Icon { get; }
    public string Label { get; }

    [ObservableProperty]
    private bool _isVisible;

    public TrackerVisibilityItem(TrackerKind kind, string icon, string label, bool isVisible, Action<TrackerKind, bool> onChanged)
    {
        Kind = kind;
        Icon = icon;
        Label = label;
        _isVisible = isVisible;
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(IsVisible)) onChanged(Kind, IsVisible);
        };
    }
}

public partial class CustomizeHomeViewModel : ObservableObject
{
    public ObservableCollection<TrackerVisibilityItem> Items { get; } = [];

    public CustomizeHomeViewModel(HomeLayoutPreferenceService homeLayout)
    {
        foreach (var kind in Enum.GetValues<TrackerKind>())
        {
            Items.Add(new TrackerVisibilityItem(
                kind, TrackerKindInfo.Icon(kind), TrackerKindInfo.Label(kind),
                homeLayout.IsVisible(kind),
                (k, visible) => homeLayout.SetVisible(k, visible)));
        }
    }
}