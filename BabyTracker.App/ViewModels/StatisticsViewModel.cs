using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BabyTracker.App.ViewModels;

public partial class StatisticsViewModel : ObservableObject
{
    public event Action? DayDetailRequested;

    [RelayCommand] private void OpenDayDetail() => DayDetailRequested?.Invoke();
}