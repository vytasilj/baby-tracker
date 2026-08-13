using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public record MomSleepListItem(MomSleepEntry Entry, string Duration, bool IsInProgress)
{
    public bool IsCompleted => !IsInProgress;
}

public partial class MomSleepListViewModel(MomSleepRepository repository) : ObservableObject
{
    public ObservableCollection<MomSleepListItem> Entries { get; } = [];

    public event Action<MomSleepEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        var entries = await repository.GetAllAsync();
        Entries.Clear();
        foreach (var e in entries)
        {
            Entries.Add(new MomSleepListItem(e, SleepFormatter.FormatDuration(e.StartTime, e.EndTime), e.EndTime is null));
        }
    }

    [RelayCommand] private void AddNew() => EditRequested?.Invoke(null);
    [RelayCommand] private void EditEntry(MomSleepListItem item) => EditRequested?.Invoke(item.Entry);
}