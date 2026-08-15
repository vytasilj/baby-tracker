using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using BabyTracker.Data;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class SleepChartViewModel : ObservableObject
{
    private const int DaysToShow = 14;

    private readonly SleepRepository _sleepRepository;
    private readonly MomSleepRepository _momSleepRepository;
    private readonly CurrentChildContext _childContext;

    [ObservableProperty] private Chart? _babyChart;
    [ObservableProperty] private Chart? _momChart;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NoData))]
    private bool _hasData;

    public bool NoData => !HasData;

    public SleepChartViewModel(SleepRepository sleepRepository, MomSleepRepository momSleepRepository, CurrentChildContext childContext)
    {
        _sleepRepository = sleepRepository;
        _momSleepRepository = momSleepRepository;
        _childContext = childContext;
        _ = RefreshAsync();
    }

    public async Task RefreshAsync()
    {
        if (_childContext.ChildId is not { } childId) return;

        var babySleeps = await _sleepRepository.GetAllAsync(childId);
        var momSleeps = await _momSleepRepository.GetAllAsync();

        var days = Enumerable.Range(0, DaysToShow)
            .Select(offset => DateOnly.FromDateTime(DateTime.Today).AddDays(-(DaysToShow - 1 - offset)))
            .ToList();

        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        var accentColor = SKColor.Parse(isDark ? "#2DD4BF" : "#0F8F7E");
        var textColor = isDark ? SKColors.White : SKColors.Black;

        BabyChart = BuildChart(days, babySleeps.Select(e => (e.StartTime, e.EndTime)), accentColor, textColor);
        MomChart = BuildChart(days, momSleeps.Select(e => (e.StartTime, e.EndTime)), accentColor, textColor);

        HasData = babySleeps.Count > 0 || momSleeps.Count > 0;
    }

    private static BarChart BuildChart(List<DateOnly> days, IEnumerable<(DateTime Start, DateTime? End)> sleeps, SKColor accentColor, SKColor textColor)
    {
        var sleepsList = sleeps.ToList();
        var entries = days.Select(day =>
        {
            var hours = SleepHoursCalculator.TotalHoursForDay(day, sleepsList, DateTime.Now);
            return new ChartEntry((float)Math.Round(hours, 1))
            {
                Label = day.ToString("d.M."),
                ValueLabel = hours.ToString("0.0"),
                Color = accentColor,
                TextColor = textColor,
                ValueLabelColor = textColor
            };
        }).ToArray();

        return new BarChart {
            Entries = entries,
            LabelTextSize = 24,
            BackgroundColor = SKColors.Transparent,
            LabelColor = textColor
        };
    }
}