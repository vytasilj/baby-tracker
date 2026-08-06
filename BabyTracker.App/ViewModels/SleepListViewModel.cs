using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record SleepListItem(SleepEntry Entry, string Duration, bool IsInProgress)
{
    public bool IsCompleted => !IsInProgress;
}

public partial class SleepListViewModel(SleepRepository repository, CurrentChildContext childContext) : ObservableObject
{
    public ObservableCollection<SleepListItem> Entries { get; } = [];

    public event Action<SleepEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        if (childContext.ChildId is not { } childId) return;

        var entries = await repository.GetAllAsync(childId);
        Entries.Clear();
        foreach (var e in entries)
        {
            Entries.Add(new SleepListItem(e, SleepFormatter.FormatDuration(e.StartTime, e.EndTime), e.EndTime is null));
        }
    }

    [RelayCommand]
    private void AddNew() => EditRequested?.Invoke(null);

    [RelayCommand]
    private void EditEntry(SleepListItem item) => EditRequested?.Invoke(item.Entry);
}