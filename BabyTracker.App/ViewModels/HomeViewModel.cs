using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public record HomeCardItem(TrackerKind Kind, string Icon, string Label, string Summary);

public partial class HomeViewModel : ObservableObject
{
    private readonly ChildRepository _childRepository;
    private readonly DailyTrackerSummaryService _summaryService;
    private readonly CalendarEventRepository _calendarRepository;
    private readonly CurrentChildContext _childContext;
    private readonly HomeLayoutPreferenceService _homeLayout;
    private AgeDescription? _ageDescription;

    [ObservableProperty] private string _childName = "";
    [ObservableProperty] private string _age = "";
    [ObservableProperty] private string _themeIcon = ThemeService.GetIconForCurrentTheme();

    public ObservableCollection<HomeCardItem> Cards { get; } = [];

    public event Action? SettingsRequested;
    public event Action? ChildrenRequested;
    public event Action? AllTrackersRequested;
    public event Action<TrackerKind>? OpenTrackerRequested;
    public event Action<TrackerKind>? AddTrackerRequested;
    public event Action? StatisticsRequested;

    public HomeViewModel(
        ChildRepository childRepository,
        DailyTrackerSummaryService summaryService,
        CalendarEventRepository calendarRepository,
        CurrentChildContext childContext,
        HomeLayoutPreferenceService homeLayout)
    {
        _childRepository = childRepository;
        _summaryService = summaryService;
        _calendarRepository = calendarRepository;
        _childContext = childContext;
        _homeLayout = homeLayout;

        _ = RefreshAsync();
        _childContext.Changed += async () => await RefreshAsync();

        LocalizationResourceManager.Instance.PropertyChanged += (_, _) =>
        {
            if (_ageDescription is not null) Age = AgeFormatter.Format(_ageDescription);
            _ = RefreshSummaryAsync();
        };
    }

    public async Task RefreshAsync()
    {
        if (_childContext.ChildId is not { } childId) return;

        var child = await _childRepository.GetByIdAsync(childId);
        if (child is null) return;

        ChildName = child.Name;
        _ageDescription = AgeCalculator.Calculate(child.BirthDate, DateOnly.FromDateTime(DateTime.Today));
        Age = AgeFormatter.Format(_ageDescription);

        await RefreshSummaryAsync();
    }

    public async Task RefreshSummaryAsync()
    {
        if (_childContext.ChildId is not { } childId) return;

        var today = DateOnly.FromDateTime(DateTime.Today);
        var cards = new List<HomeCardItem>();

        foreach (var kind in Enum.GetValues<TrackerKind>().Where(_homeLayout.IsVisible))
        {
            var summary = await ComputeSummaryAsync(kind, childId, today);
            cards.Add(new HomeCardItem(kind, TrackerKindInfo.Icon(kind), TrackerKindInfo.Label(kind), summary));
        }

        Cards.Clear();
        foreach (var c in cards) Cards.Add(c);
    }

    private async Task<string> ComputeSummaryAsync(TrackerKind kind, Guid childId, DateOnly today)
    {
        if (kind == TrackerKind.Calendar)
        {
            var events = await _calendarRepository.GetAllAsync();
            var next = events.FirstOrDefault(e => e.OccursAt >= DateTime.Now);
            return next is null ? "—" : next.OccursAt.ToString("d.M. HH:mm");
        }

        return await _summaryService.ComputeSummaryAsync(kind, childId, today);
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeService.ToggleTheme();
        ThemeIcon = ThemeService.GetIconForCurrentTheme();
    }

    [RelayCommand] private void OpenSettings() => SettingsRequested?.Invoke();
    [RelayCommand] private void OpenChildren() => ChildrenRequested?.Invoke();
    [RelayCommand] private void OpenAllTrackers() => AllTrackersRequested?.Invoke();
    [RelayCommand] private void OpenTracker(HomeCardItem card) => OpenTrackerRequested?.Invoke(card.Kind);
    [RelayCommand] private void AddTracker(HomeCardItem card) => AddTrackerRequested?.Invoke(card.Kind);
    [RelayCommand] private void OpenStatistics() => StatisticsRequested?.Invoke();
}