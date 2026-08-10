using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly ChildRepository _childRepository;
    private readonly EntryRepository<FeedingEntry> _feedingRepository;
    private readonly SleepRepository _sleepRepository;
    private readonly EntryRepository<DiaperEntry> _diaperRepository;
    private readonly CurrentChildContext _childContext;
    private AgeDescription? _ageDescription;

    [ObservableProperty] private string _childName = "";
    [ObservableProperty] private string _age = "";
    [ObservableProperty] private string _themeIcon = ThemeService.GetIconForCurrentTheme();

    [ObservableProperty] private string _feedingSummary = "—";
    [ObservableProperty] private string _sleepSummary = "—";
    [ObservableProperty] private string _diaperSummary = "—";

    public event Action? SettingsRequested;
    public event Action? ChildrenRequested;
    public event Action? DiapersRequested;
    public event Action? FeedingRequested;
    public event Action? SleepRequested;
    public event Action? AddDiaperRequested;
    public event Action? AddFeedingRequested;
    public event Action? AddSleepRequested;
    public event Action? AllTrackersRequested;

    public HomeViewModel(
        ChildRepository childRepository,
        EntryRepository<FeedingEntry> feedingRepository,
        SleepRepository sleepRepository,
        EntryRepository<DiaperEntry> diaperRepository,
        CurrentChildContext childContext)
    {
        _childRepository = childRepository;
        _feedingRepository = feedingRepository;
        _sleepRepository = sleepRepository;
        _diaperRepository = diaperRepository;
        _childContext = childContext;

        _ = RefreshAsync();
        _childContext.Changed += async () => await RefreshAsync();

        LocalizationResourceManager.Instance.PropertyChanged += (_, _) =>
        {
            if (_ageDescription is not null) Age = AgeFormatter.Format(_ageDescription);
            _ = RefreshSummaryAsync(); // "hours"/"minutes" wording depends on the current language too
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
        var feedings = await _feedingRepository.GetAllAsync(childId);
        var sleeps = await _sleepRepository.GetAllAsync(childId);
        var diapers = await _diaperRepository.GetAllAsync(childId);

        var summary = DailySummaryCalculator.Calculate(today, feedings, sleeps, diapers, DateTime.Now);

        FeedingSummary = $"{summary.FeedingCount}×";
        SleepSummary = SleepFormatter.FormatTotalHours(summary.SleepHours);
        DiaperSummary = $"{summary.DiaperCount}×";
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeService.ToggleTheme();
        ThemeIcon = ThemeService.GetIconForCurrentTheme();
    }

    [RelayCommand] private void OpenSettings() => SettingsRequested?.Invoke();
    [RelayCommand] private void OpenChildren() => ChildrenRequested?.Invoke();
    [RelayCommand] private void OpenDiapers() => DiapersRequested?.Invoke();
    [RelayCommand] private void OpenFeeding() => FeedingRequested?.Invoke();
    [RelayCommand] private void OpenSleep() => SleepRequested?.Invoke();
    [RelayCommand] private void AddDiaper() => AddDiaperRequested?.Invoke();
    [RelayCommand] private void AddFeeding() => AddFeedingRequested?.Invoke();
    [RelayCommand] private void AddSleep() => AddSleepRequested?.Invoke();
    [RelayCommand] private void OpenAllTrackers() => AllTrackersRequested?.Invoke();
}