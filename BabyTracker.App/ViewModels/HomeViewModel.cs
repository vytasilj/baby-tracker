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
    private readonly EntryRepository<FeedingEntry> _feedingRepository;
    private readonly SleepRepository _sleepRepository;
    private readonly EntryRepository<DiaperEntry> _diaperRepository;
    private readonly EntryRepository<TemperatureEntry> _temperatureRepository;
    private readonly EntryRepository<WeightEntry> _weightRepository;
    private readonly EntryRepository<PumpingEntry> _pumpingRepository;
    private readonly SupplementRepository _supplementRepository;
    private readonly MomSleepRepository _momSleepRepository;
    private readonly CurrentChildContext _childContext;
    private readonly UnitPreferenceService _unitPreference;
    private readonly HomeLayoutPreferenceService _homeLayout;
    private readonly CalendarEventRepository _calendarRepository;
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

    public HomeViewModel(
        ChildRepository childRepository,
        EntryRepository<FeedingEntry> feedingRepository,
        SleepRepository sleepRepository,
        EntryRepository<DiaperEntry> diaperRepository,
        EntryRepository<TemperatureEntry> temperatureRepository,
        EntryRepository<WeightEntry> weightRepository,
        EntryRepository<PumpingEntry> pumpingRepository,
        SupplementRepository supplementRepository,
        MomSleepRepository momSleepRepository,
        CurrentChildContext childContext,
        UnitPreferenceService unitPreference,
        HomeLayoutPreferenceService homeLayout,
        CalendarEventRepository calendarRepository)
    {
        _childRepository = childRepository;
        _feedingRepository = feedingRepository;
        _sleepRepository = sleepRepository;
        _diaperRepository = diaperRepository;
        _temperatureRepository = temperatureRepository;
        _weightRepository = weightRepository;
        _pumpingRepository = pumpingRepository;
        _supplementRepository = supplementRepository;
        _momSleepRepository = momSleepRepository;
        _childContext = childContext;
        _unitPreference = unitPreference;
        _homeLayout = homeLayout;
        _calendarRepository = calendarRepository;

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

    // Event-based trackers (logged multiple times a day) show today's count.
    // Value-based trackers (Temperature/Weight, logged occasionally) show the most recent reading instead.
    private async Task<string> ComputeSummaryAsync(TrackerKind kind, Guid childId, DateOnly today)
    {
        var loc = LocalizationResourceManager.Instance;

        switch (kind)
        {
            case TrackerKind.Feeding:
                {
                    var entries = await _feedingRepository.GetAllAsync(childId);
                    return $"{entries.Count(e => DateOnly.FromDateTime(e.OccurredAt) == today)}×";
                }
            case TrackerKind.Diaper:
                {
                    var entries = await _diaperRepository.GetAllAsync(childId);
                    return $"{entries.Count(e => DateOnly.FromDateTime(e.OccurredAt) == today)}×";
                }
            case TrackerKind.Pumping:
                {
                    var entries = await _pumpingRepository.GetAllAsync(childId);
                    return $"{entries.Count(e => DateOnly.FromDateTime(e.OccurredAt) == today)}×";
                }
            case TrackerKind.Supplement:
                {
                    var entries = await _supplementRepository.GetEntriesAsync(childId);
                    return $"{entries.Count(e => DateOnly.FromDateTime(e.OccurredAt) == today)}×";
                }
            case TrackerKind.Sleep:
                {
                    var entries = await _sleepRepository.GetAllAsync(childId);
                    var hours = SleepHoursCalculator.TotalHoursForDay(today, entries.Select(e => (e.StartTime, e.EndTime)), DateTime.Now);
                    return SleepFormatter.FormatTotalHours(hours);
                }
            case TrackerKind.MomSleep:
                {
                    var entries = await _momSleepRepository.GetAllAsync();
                    var hours = SleepHoursCalculator.TotalHoursForDay(today, entries.Select(e => (e.StartTime, e.EndTime)), DateTime.Now);
                    return SleepFormatter.FormatTotalHours(hours);
                }
            case TrackerKind.Temperature:
                {
                    var entries = await _temperatureRepository.GetAllAsync(childId);
                    return entries.Count == 0 ? "—" : TemperatureFormatter.FormatForDisplay(entries[0].ValueCelsius, _unitPreference.Current, loc.NumberFormatCulture);
                }
            case TrackerKind.Weight:
                {
                    var entries = await _weightRepository.GetAllAsync(childId);
                    return entries.Count == 0 ? "—" : WeightFormatter.FormatForDisplay(entries[0].WeightKg, _unitPreference.Current, loc.NumberFormatCulture);
                }
            case TrackerKind.Calendar:
                {
                    var events = await _calendarRepository.GetAllAsync();
                    var next = events.FirstOrDefault(e => e.OccursAt >= DateTime.Now);
                    return next is null ? "—" : next.OccursAt.ToString("d.M. HH:mm");
                }
            default:
                return "—";
        }
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
}