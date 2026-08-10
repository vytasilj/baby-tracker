using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class WeightEntryViewModel(EntryRepository<WeightEntry> repository, CurrentChildContext childContext) : ObservableObject
{
    private Guid? _entryId;
    private DateTime _createdAt;

    [ObservableProperty] private DateTime _entryDate = DateTime.Today;
    [ObservableProperty] private TimeSpan _entryTime = DateTime.Now.TimeOfDay;
    [ObservableProperty] private double _value = 3.5;
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private bool _isSaving;

    public string UnitLabel => "kg";

    public event Action? Completed;

    public void LoadEntry(WeightEntry? entry)
    {
        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            EntryDate = DateTime.Today;
            EntryTime = DateTime.Now.TimeOfDay;
            Value = 3.5;
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        _createdAt = entry.CreatedAt;
        IsEditing = true;
        EntryDate = entry.OccurredAt.Date;
        EntryTime = entry.OccurredAt.TimeOfDay;
        Value = (double)entry.WeightKg;
        Notes = entry.Notes ?? "";
    }

    [RelayCommand]
    private async Task Save()
    {
        if (childContext.ChildId is not { } childId) return;

        IsSaving = true;
        try
        {
            var occurredAt = EntryDate.Date + EntryTime;
            var kg = (decimal)Value;

            if (_entryId is { } id)
            {
                await repository.UpdateAsync(new WeightEntry
                {
                    Id = id,
                    ChildId = childId,
                    CreatedAt = _createdAt,
                    OccurredAt = occurredAt,
                    WeightKg = kg,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
                });
            }
            else
            {
                await repository.AddAsync(new WeightEntry
                {
                    ChildId = childId,
                    OccurredAt = occurredAt,
                    WeightKg = kg,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
                });
            }
            await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Saved"]);
            Completed?.Invoke();
        }
        finally { IsSaving = false; }
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (_entryId is { } id) await repository.DeleteAsync(id);
        await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Deleted"]);
        Completed?.Invoke();
    }
}