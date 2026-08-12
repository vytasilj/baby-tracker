using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record SupplementListItem(SupplementEntry Entry, string Summary);

public partial class SupplementListViewModel(SupplementRepository repository, CurrentChildContext childContext) : ObservableObject
{
    public ObservableCollection<SupplementListItem> Entries { get; } = [];

    public event Action<SupplementEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        if (childContext.ChildId is not { } childId) return;

        var entries = await repository.GetEntriesAsync(childId);
        Entries.Clear();
        foreach (var e in entries)
        {
            var summary = string.Join(", ", e.Supplements.Select(SupplementFormatter.DisplayName));
            Entries.Add(new SupplementListItem(e, string.IsNullOrEmpty(summary) ? "—" : summary));
        }
    }

    [RelayCommand] private void AddNew() => EditRequested?.Invoke(null);
    [RelayCommand] private void EditEntry(SupplementListItem item) => EditRequested?.Invoke(item.Entry);
}