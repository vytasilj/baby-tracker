using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class SupplementEntryViewModel(SupplementRepository repository, CurrentChildContext childContext) : ObservableObject
{
    private Guid? _entryId;

    public ObservableCollection<SelectableSupplement> Supplements { get; } = [];

    [ObservableProperty] private DateTime _entryDate = DateTime.Today;
    [ObservableProperty] private TimeSpan _entryTime = DateTime.Now.TimeOfDay;
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private string _newCustomName = "";
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private bool _isSaving;

    public event Action? Completed;

    public async Task LoadEntryAsync(SupplementEntry? entry)
    {
        var selectedIds = entry?.Supplements.Select(s => s.Id).ToHashSet() ?? [];

        var definitions = await repository.GetDefinitionsAsync();
        Supplements.Clear();
        foreach (var def in definitions)
        {
            Supplements.Add(new SelectableSupplement(def, SupplementFormatter.DisplayName(def), selectedIds.Contains(def.Id)));
        }

        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            EntryDate = DateTime.Today;
            EntryTime = DateTime.Now.TimeOfDay;
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        IsEditing = true;
        EntryDate = entry.OccurredAt.Date;
        EntryTime = entry.OccurredAt.TimeOfDay;
        Notes = entry.Notes ?? "";
    }

    [RelayCommand]
    private async Task AddCustom()
    {
        var name = NewCustomName.Trim();
        if (string.IsNullOrWhiteSpace(name)) return;

        var def = await repository.AddCustomDefinitionAsync(name);
        Supplements.Add(new SelectableSupplement(def, name, isSelected: true));
        NewCustomName = "";
    }

    [RelayCommand]
    private async Task Save()
    {
        if (childContext.ChildId is not { } childId) return;

        IsSaving = true;
        try
        {
            var occurredAt = EntryDate.Date + EntryTime;
            var selectedIds = Supplements.Where(s => s.IsSelected).Select(s => s.Definition.Id).ToList();
            var notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim();

            if (_entryId is { } id)
            {
                await repository.UpdateEntryAsync(id, occurredAt, selectedIds, notes);
            }
            else
            {
                await repository.AddEntryAsync(childId, occurredAt, selectedIds, notes);
            }
            await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Saved"]);
            Completed?.Invoke();
        }
        finally { IsSaving = false; }
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (_entryId is { } id) await repository.DeleteEntryAsync(id);
        await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Deleted"]);
        Completed?.Invoke();
    }

    public async Task HideSupplementConfirmedAsync(SelectableSupplement item)
    {
        await repository.HideDefinitionAsync(item.Definition.Id);
        Supplements.Remove(item);
    }
}