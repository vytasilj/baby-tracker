using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record MedicalExamListItem(MedicalExamEntry Entry, string ExamName);

public partial class MedicalExamListViewModel(EntryRepository<MedicalExamEntry> repository, CurrentChildContext childContext) : ObservableObject
{
    public ObservableCollection<MedicalExamListItem> Entries { get; } = [];

    public event Action<MedicalExamEntry?>? EditRequested;

    [RelayCommand]
    private async Task Load()
    {
        if (childContext.ChildId is not { } childId) return;

        var entries = await repository.GetAllAsync(childId);
        Entries.Clear();
        foreach (var e in entries)
        {
            Entries.Add(new MedicalExamListItem(e, MedicalExamFormatter.DisplayName(e)));
        }
    }

    [RelayCommand] private void AddNew() => EditRequested?.Invoke(null);
    [RelayCommand] private void EditEntry(MedicalExamListItem item) => EditRequested?.Invoke(item.Entry);
}