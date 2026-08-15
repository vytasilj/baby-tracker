using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public enum ChartRange { Week, Month, All }

public partial class WeightChartViewModel : ObservableObject
{
    private readonly EntryRepository<WeightEntry> _repository;
    private readonly CurrentChildContext _childContext;
    private readonly UnitPreferenceService _unitPreference;

    [ObservableProperty] private Chart? _chart;
    [ObservableProperty] private ChartRange _selectedRange = ChartRange.Month;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NoData))]
    private bool _hasData;

    public bool NoData => !HasData;

    public WeightChartViewModel(EntryRepository<WeightEntry> repository, CurrentChildContext childContext, UnitPreferenceService unitPreference)
    {
        _repository = repository;
        _childContext = childContext;
        _unitPreference = unitPreference;
        _ = RefreshAsync();
    }

    partial void OnSelectedRangeChanged(ChartRange value) => _ = RefreshAsync();

    [RelayCommand] private void SelectWeek() => SelectedRange = ChartRange.Week;
    [RelayCommand] private void SelectMonth() => SelectedRange = ChartRange.Month;
    [RelayCommand] private void SelectAll() => SelectedRange = ChartRange.All;

    public async Task RefreshAsync()
    {
        if (_childContext.ChildId is not { } childId) return;

        var entries = await _repository.GetAllAsync(childId); // newest first
        var cutoff = SelectedRange switch
        {
            ChartRange.Week => DateTime.Today.AddDays(-7),
            ChartRange.Month => DateTime.Today.AddMonths(-1),
            _ => DateTime.MinValue
        };

        var filtered = entries
            .Where(e => e.OccurredAt >= cutoff)
            .OrderBy(e => e.OccurredAt) // chart needs chronological order
            .ToList();

        HasData = filtered.Count > 0;
        if (!HasData)
        {
            Chart = null;
            return;
        }

        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        var accentColor = SKColor.Parse(isDark ? "#2DD4BF" : "#0F8F7E");

        var chartEntries = filtered.Select(e =>
        {
            var displayValue = WeightFormatter.ToDisplayValue(e.WeightKg, _unitPreference.Current);
            return new ChartEntry((float)displayValue)
            {
                Label = e.OccurredAt.ToString("d.M."),
                ValueLabel = displayValue.ToString("0.0"),
                Color = accentColor
            };
        }).ToArray();

        Chart = new LineChart
        {
            Entries = chartEntries,
            LineMode = LineMode.Straight,
            PointMode = PointMode.Circle,
            LabelTextSize = 28
        };
    }
}