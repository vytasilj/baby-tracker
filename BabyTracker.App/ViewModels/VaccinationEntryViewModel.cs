using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record VaccineOption(VaccineDefinition Definition, string DisplayName);

public partial class VaccinationEntryViewModel : ObservableObject
{
    private readonly VaccinationRepository _repository;
    private readonly CurrentChildContext _childContext;
    private Guid? _entryId;
    private DateTime _createdAt;

    public ObservableCollection<VaccineOption> VaccineOptions { get; } = [];

    [ObservableProperty] private VaccineOption? _selectedVaccine;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowAddToCalendar))]
    [NotifyPropertyChangedFor(nameof(DateLabel))]
    private bool _isGiven = true;

    [ObservableProperty] private DateTime _entryDate = DateTime.Today;
    [ObservableProperty] private int? _doseNumber;
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private string _newCustomName = "";
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private bool _isSaving;

    public bool ShowAddToCalendar => !IsGiven;
    public string DateLabel => LocalizationResourceManager.Instance[IsGiven ? "Vaccination_DateGivenLabel" : "Vaccination_DueDateLabel"];

    public event Action? Completed;

    public VaccinationEntryViewModel(VaccinationRepository repository, CurrentChildContext childContext)
    {
        _repository = repository;
        _childContext = childContext;
        // Deliberately NOT loading vaccine options here. LoadEntryAsync (always
        // called explicitly right after this ViewModel is constructed) does that.
        // Loading here too created a race between two concurrent DB calls, which
        // intermittently reset SelectedVaccine to null after the page appeared.
    }

    private async Task LoadVaccineOptionsAsync()
    {
        var definitions = await _repository.GetDefinitionsAsync();
        VaccineOptions.Clear();
        foreach (var d in definitions)
        {
            VaccineOptions.Add(new VaccineOption(d, VaccineFormatter.DisplayName(d)));
        }
    }

    public async Task LoadEntryAsync(VaccinationEntry? entry)
    {
        await LoadVaccineOptionsAsync();

        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            SelectedVaccine = VaccineOptions.FirstOrDefault();
            IsGiven = true;
            EntryDate = DateTime.Today;
            DoseNumber = null;
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        _createdAt = entry.CreatedAt;
        IsEditing = true;
        SelectedVaccine = VaccineOptions.FirstOrDefault(o => o.Definition.Id == entry.VaccineDefinitionId);
        IsGiven = entry.IsGiven;
        EntryDate = entry.IsGiven ? entry.OccurredAt.Date : entry.DueDate!.Value.ToDateTime(TimeOnly.MinValue);
        DoseNumber = entry.DoseNumber;
        Notes = entry.Notes ?? "";
    }

    [RelayCommand]
    private async Task AddCustom()
    {
        var name = NewCustomName.Trim();
        if (string.IsNullOrWhiteSpace(name)) return;

        var def = await _repository.AddCustomDefinitionAsync(name);
        var option = new VaccineOption(def, name);
        VaccineOptions.Add(option);
        SelectedVaccine = option;
        NewCustomName = "";
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_childContext.ChildId is not { } childId || SelectedVaccine is null) return;

        IsSaving = true;
        try
        {
            var date = DateOnly.FromDateTime(EntryDate);
            var notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim();

            if (_entryId is { } id)
            {
                await _repository.UpdateAsync(new VaccinationEntry
                {
                    Id = id,
                    ChildId = childId,
                    CreatedAt = _createdAt,
                    VaccineDefinitionId = SelectedVaccine.Definition.Id,
                    OccurredAt = IsGiven ? date.ToDateTime(TimeOnly.MinValue) : DateTime.Today,
                    DueDate = IsGiven ? null : date,
                    DoseNumber = DoseNumber,
                    Notes = notes
                });
            }
            else
            {
                await _repository.AddAsync(new VaccinationEntry
                {
                    ChildId = childId,
                    VaccineDefinitionId = SelectedVaccine.Definition.Id,
                    OccurredAt = IsGiven ? date.ToDateTime(TimeOnly.MinValue) : DateTime.Today,
                    DueDate = IsGiven ? null : date,
                    DoseNumber = DoseNumber,
                    Notes = notes
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

    [RelayCommand]
    private void AddToDeviceCalendar()
    {
        if (SelectedVaccine is null) return;
        var title = $"{LocalizationResourceManager.Instance["Vaccination_Title"]}: {SelectedVaccine.DisplayName}";
        CalendarIntentHelper.AddToDeviceCalendar(title, EntryDate, Notes);
    }
}