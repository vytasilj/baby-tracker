using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public record CalendarListItem(CalendarEvent Entry, string ChildLabel);

public partial class CalendarListViewModel(CalendarEventRepository repository, ChildRepository childRepository) : ObservableObject
{
    public ObservableCollection<CalendarListItem> Entries { get; } = [];

    public event Action<CalendarEvent?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        var entries = await repository.GetAllAsync();
        var children = await childRepository.GetAllAsync();

        Entries.Clear();
        foreach (var e in entries)
        {
            var childLabel = e.ChildId is { } id
                ? children.FirstOrDefault(c => c.Id == id)?.Name ?? ""
                : LocalizationResourceManager.Instance["Calendar_FamilyWide"];
            Entries.Add(new CalendarListItem(e, childLabel));
        }
    }

    [RelayCommand] private void AddNew() => EditRequested?.Invoke(null);
    [RelayCommand] private void EditEntry(CalendarListItem item) => EditRequested?.Invoke(item.Entry);
}