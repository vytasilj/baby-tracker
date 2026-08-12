using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public enum TrackerKind { Feeding, Sleep, Diaper, Temperature, Weight, Pumping }

public record TrackerListItem(TrackerKind Kind, string Icon, string Label);

public partial class AllTrackersViewModel : ObservableObject
{
    public List<TrackerListItem> Trackers { get; }

    public event Action<TrackerKind>? TrackerSelected;

    public AllTrackersViewModel()
    {
        var loc = LocalizationResourceManager.Instance;
        Trackers =
        [
            new(TrackerKind.Feeding, "🍼", loc["Feeding_Title"]),
            new(TrackerKind.Sleep, "😴", loc["Sleep_Title"]),
            new(TrackerKind.Diaper, "🧷", loc["Diaper_Title"]),
            new(TrackerKind.Temperature, "🌡️", loc["Temperature_Title"]),
            new(TrackerKind.Weight, "⚖️", loc["Weight_Title"]),
            new(TrackerKind.Pumping, "🤱", loc["Pumping_Title"]),
        ];
    }

    [RelayCommand]
    private void Select(TrackerListItem item) => TrackerSelected?.Invoke(item.Kind);
}