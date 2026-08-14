using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record VaccinationListItem(VaccinationEntry Entry, string VaccineName, string StatusLabel, bool IsDue);

public partial class VaccinationListViewModel(VaccinationRepository repository, CurrentChildContext childContext) : ObservableObject
{
    public ObservableCollection<VaccinationListItem> Entries { get; } = [];

    public event Action<VaccinationEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        if (childContext.ChildId is not { } childId) return;

        var loc = LocalizationResourceManager.Instance;
        var entries = await repository.GetAllAsync(childId);

        var items = entries.Select(e =>
        {
            var name = VaccineFormatter.DisplayName(e.Vaccine!);
            var status = e.IsGiven
                ? $"{loc["Vaccination_Given"]} {e.OccurredAt:d.M.yyyy}"
                : $"{loc["Vaccination_Due"]} {e.DueDate:d.M.yyyy}";
            return new VaccinationListItem(e, name, status, !e.IsGiven);
        })
        // Due (upcoming) entries first, soonest due date first; given entries after, most recent first.
        .OrderBy(i => i.IsDue ? 0 : 1)
        .ThenBy(i => i.IsDue ? i.Entry.DueDate!.Value.DayNumber : 0)
        .ThenByDescending(i => i.IsDue ? default : i.Entry.OccurredAt)
        .ToList();

        Entries.Clear();
        foreach (var i in items) Entries.Add(i);
    }

    [RelayCommand] private void AddNew() => EditRequested?.Invoke(null);
    [RelayCommand] private void EditEntry(VaccinationListItem item) => EditRequested?.Invoke(item.Entry);
}