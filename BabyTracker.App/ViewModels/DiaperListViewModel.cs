using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class DiaperListViewModel(DiaperRepository repository, CurrentChildContext childContext) : ObservableObject
{
    public ObservableCollection<DiaperEntry> Entries { get; } = [];

    public event Action<DiaperEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        if (childContext.ChildId is not { } childId) return;

        var entries = await repository.GetAllAsync(childId);
        Entries.Clear();
        foreach (var e in entries) Entries.Add(e);
    }

    [RelayCommand]
    private void AddNew() => EditRequested?.Invoke(null);

    [RelayCommand]
    private void EditEntry(DiaperEntry entry) => EditRequested?.Invoke(entry);
}