using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BabyTracker.App.ViewModels;

public record TrackerListItem(TrackerKind Kind, string Icon, string Label);

public partial class AllTrackersViewModel : ObservableObject
{
    public List<TrackerListItem> Trackers { get; }

    public event Action<TrackerKind>? TrackerSelected;

    public AllTrackersViewModel()
    {
        Trackers = Enum.GetValues<TrackerKind>()
            .Select(k => new TrackerListItem(k, TrackerKindInfo.Icon(k), TrackerKindInfo.Label(k)))
            .ToList();
    }

    [RelayCommand]
    private void Select(TrackerListItem item) => TrackerSelected?.Invoke(item.Kind);
}