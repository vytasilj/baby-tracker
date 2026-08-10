using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class TemperatureEntryViewModel : ObservableObject
{
    private readonly EntryRepository<TemperatureEntry> _repository;
    private readonly CurrentChildContext _childContext;
    private readonly UnitPreferenceService _unitPreference;
    private Guid? _entryId;
    private DateTime _createdAt;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ValueDisplay))]
    private double _value;

    [ObservableProperty] private DateTime _entryDate = DateTime.Today;
    [ObservableProperty] private TimeSpan _entryTime = DateTime.Now.TimeOfDay;
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private bool _isSaving;

    public string UnitLabel => TemperatureFormatter.UnitLabel(_unitPreference.Current);
    public string ValueDisplay => $"{Value.ToString("0.0", LocalizationResourceManager.Instance.NumberFormatCulture)} {UnitLabel}";
    public double StepperMinimum => TemperatureFormatter.StepperMinimum(_unitPreference.Current);
    public double StepperMaximum => TemperatureFormatter.StepperMaximum(_unitPreference.Current);
    public double StepperIncrement => TemperatureFormatter.StepperIncrement(_unitPreference.Current);

    public event Action? Completed;

    public TemperatureEntryViewModel(EntryRepository<TemperatureEntry> repository, CurrentChildContext childContext, UnitPreferenceService unitPreference)
    {
        _repository = repository;
        _childContext = childContext;
        _unitPreference = unitPreference;
        Value = TemperatureFormatter.ToDisplayValue(36.6m, _unitPreference.Current);
    }

    public void LoadEntry(TemperatureEntry? entry)
    {
        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            EntryDate = DateTime.Today;
            EntryTime = DateTime.Now.TimeOfDay;
            Value = TemperatureFormatter.ToDisplayValue(36.6m, _unitPreference.Current);
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        _createdAt = entry.CreatedAt;
        IsEditing = true;
        EntryDate = entry.OccurredAt.Date;
        EntryTime = entry.OccurredAt.TimeOfDay;
        Value = TemperatureFormatter.ToDisplayValue(entry.ValueCelsius, _unitPreference.Current);
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
            var celsius = TemperatureFormatter.ToCanonicalCelsius(Value, _unitPreference.Current);

            if (_entryId is { } id)
            {
                await _repository.UpdateAsync(new TemperatureEntry
                {
                    Id = id,
                    ChildId = childId,
                    CreatedAt = _createdAt,
                    OccurredAt = occurredAt,
                    ValueCelsius = celsius,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
                });
            }
            else
            {
                await _repository.AddAsync(new TemperatureEntry
                {
                    ChildId = childId,
                    OccurredAt = occurredAt,
                    ValueCelsius = celsius,
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