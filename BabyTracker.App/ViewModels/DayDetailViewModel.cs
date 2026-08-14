using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class DayDetailViewModel : ObservableObject
{
    private static readonly TrackerKind[] DaySummaryTrackers =
    [
        TrackerKind.Feeding, TrackerKind.Sleep, TrackerKind.Diaper,
        TrackerKind.Temperature, TrackerKind.Weight, TrackerKind.Pumping,
        TrackerKind.Supplement, TrackerKind.MomSleep
    ];

    private readonly DailyTrackerSummaryService _summaryService;
    private readonly CurrentChildContext _childContext;

    public ObservableCollection<HomeCardItem> Cards { get; } = [];

    [ObservableProperty] private DateTime _selectedDate = DateTime.Today;

    public event Action<TrackerKind>? OpenTrackerRequested;

    public DayDetailViewModel(DailyTrackerSummaryService summaryService, CurrentChildContext childContext)
    {
        _summaryService = summaryService;
        _childContext = childContext;
        _ = RefreshAsync();
    }

    partial void OnSelectedDateChanged(DateTime value) => _ = RefreshAsync();

    private async Task RefreshAsync()
    {
        if (_childContext.ChildId is not { } childId) return;

        var day = DateOnly.FromDateTime(SelectedDate);
        var cards = new List<HomeCardItem>();
        foreach (var kind in DaySummaryTrackers)
        {
            var summary = await _summaryService.ComputeSummaryAsync(kind, childId, day);
            cards.Add(new HomeCardItem(kind, TrackerKindInfo.Icon(kind), TrackerKindInfo.Label(kind), summary));
        }

        Cards.Clear();
        foreach (var c in cards) Cards.Add(c);
    }

    [RelayCommand] private void PreviousDay() => SelectedDate = SelectedDate.AddDays(-1);
    [RelayCommand] private void NextDay() => SelectedDate = SelectedDate.AddDays(1);
    [RelayCommand] private void OpenTracker(HomeCardItem card) => OpenTrackerRequested?.Invoke(card.Kind);
}