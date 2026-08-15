using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BabyTracker.App.ViewModels;

public partial class StatisticsViewModel : ObservableObject
{
    public event Action? DayDetailRequested;
    public event Action? WeightChartRequested;

    [RelayCommand] private void OpenDayDetail() => DayDetailRequested?.Invoke();
    [RelayCommand] private void OpenWeightChart() => WeightChartRequested?.Invoke();
}