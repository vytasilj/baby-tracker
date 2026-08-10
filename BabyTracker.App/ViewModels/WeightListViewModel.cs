using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record WeightListItem(WeightEntry Entry, string Display);

public partial class WeightListViewModel(EntryRepository<WeightEntry> repository, CurrentChildContext childContext) : ObservableObject
{
    public ObservableCollection<WeightListItem> Entries { get; } = [];

    public event Action<WeightEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        if (childContext.ChildId is not { } childId) return;

        var entries = await repository.GetAllAsync(childId);
        Entries.Clear();
        foreach (var e in entries)
        {
            Entries.Add(new WeightListItem(e, $"{e.WeightKg:0.000} kg"));
        }
    }

    [RelayCommand] private void AddNew() => EditRequested?.Invoke(null);
    [RelayCommand] private void EditEntry(WeightListItem item) => EditRequested?.Invoke(item.Entry);
}