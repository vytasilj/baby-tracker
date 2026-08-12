using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record PumpingListItem(PumpingEntry Entry, string Summary);

public partial class PumpingListViewModel(EntryRepository<PumpingEntry> repository, CurrentChildContext childContext) : ObservableObject
{
    public ObservableCollection<PumpingListItem> Entries { get; } = [];

    public event Action<PumpingEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        if (childContext.ChildId is not { } childId) return;

        var entries = await repository.GetAllAsync(childId);
        Entries.Clear();
        foreach (var e in entries)
        {
            var side = FeedingFormatter.FormatSide(e.Side);
            var summary = e.AmountMl is { } ml ? $"{side} · {ml} ml" : side;
            Entries.Add(new PumpingListItem(e, summary));
        }
    }

    [RelayCommand] private void AddNew() => EditRequested?.Invoke(null);
    [RelayCommand] private void EditEntry(PumpingListItem item) => EditRequested?.Invoke(item.Entry);
}