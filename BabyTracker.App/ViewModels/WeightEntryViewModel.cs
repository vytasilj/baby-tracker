using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class WeightEntryViewModel : ObservableObject
{
    private readonly EntryRepository<WeightEntry> _repository;
    private readonly CurrentChildContext _childContext;
    private readonly UnitPreferenceService _unitPreference;
    private Guid? _entryId;
    private DateTime _createdAt;

    [ObservableProperty] private DateTime _entryDate = DateTime.Today;
    [ObservableProperty] private TimeSpan _entryTime = DateTime.Now.TimeOfDay;
    [ObservableProperty] private double _value;
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private bool _isSaving;

    public string UnitLabel => WeightFormatter.UnitLabel(_unitPreference.Current);

    public event Action? Completed;

    public WeightEntryViewModel(EntryRepository<WeightEntry> repository, CurrentChildContext childContext, UnitPreferenceService unitPreference)
    {
        _repository = repository;
        _childContext = childContext;
        _unitPreference = unitPreference;
        Value = WeightFormatter.ToDisplayValue(3.5m, _unitPreference.Current);
    }

    public void LoadEntry(WeightEntry? entry)
    {
        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            EntryDate = DateTime.Today;
            EntryTime = DateTime.Now.TimeOfDay;
            Value = WeightFormatter.ToDisplayValue(3.5m, _unitPreference.Current);
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        _createdAt = entry.CreatedAt;
        IsEditing = true;
        EntryDate = entry.OccurredAt.Date;
        EntryTime = entry.OccurredAt.TimeOfDay;
        Value = WeightFormatter.ToDisplayValue(entry.WeightKg, _unitPreference.Current);
        Notes = entry.Notes ?? "";
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_childContext.ChildId is not { } childId) return;

        IsSaving = true;
        try
        {
            var occurredAt = EntryDate.Date + EntryTime;
            var kg = WeightFormatter.ToCanonicalKg(Value, _unitPreference.Current);

            if (_entryId is { } id)
            {
                await _repository.UpdateAsync(new WeightEntry
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
                await _repository.AddAsync(new WeightEntry
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
        if (_entryId is { } id) await _repository.DeleteAsync(id);
        await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Deleted"]);
        Completed?.Invoke();
    }
}