using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record FeedingListItem(FeedingEntry Entry, string Summary);

public partial class FeedingListViewModel(EntryRepository<FeedingEntry> repository, CurrentChildContext childContext) : ObservableObject
{
    public ObservableCollection<FeedingListItem> Entries { get; } = [];

    public event Action<FeedingEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        if (childContext.ChildId is not { } childId) return;

        var entries = await repository.GetAllAsync(childId);
        Entries.Clear();
        foreach (var e in entries)
        {
            var summary = FeedingFormatter.FormatTypeAndDetail(e.Type, e.Side, e.AmountMl);
            Entries.Add(new FeedingListItem(e, summary));
        }
    }

    [RelayCommand]
    private void AddNew() => EditRequested?.Invoke(null);

    [RelayCommand]
    private void EditEntry(FeedingListItem item) => EditRequested?.Invoke(item.Entry);
}