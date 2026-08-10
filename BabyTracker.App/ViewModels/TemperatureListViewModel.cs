using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record TemperatureListItem(TemperatureEntry Entry, string Display);

public partial class TemperatureListViewModel(EntryRepository<TemperatureEntry> repository, CurrentChildContext childContext) : ObservableObject
{
    public ObservableCollection<TemperatureListItem> Entries { get; } = [];

    public event Action<TemperatureEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        if (childContext.ChildId is not { } childId) return;

        var entries = await repository.GetAllAsync(childId);
        Entries.Clear();
        foreach (var e in entries)
        {
            Entries.Add(new TemperatureListItem(e, $"{e.ValueCelsius:0.0} °C"));
        }
    }

    [RelayCommand] private void AddNew() => EditRequested?.Invoke(null);
    [RelayCommand] private void EditEntry(TemperatureListItem item) => EditRequested?.Invoke(item.Entry);
}