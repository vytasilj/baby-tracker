using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record DiaperTypeOption(DiaperType Value, string Display);

public partial class DiaperEntryViewModel : ObservableObject
{
    private readonly EntryRepository<DiaperEntry> _repository;
    private readonly CurrentChildContext _childContext;
    private Guid? _entryId;

    public List<DiaperTypeOption> TypeOptions { get; }

    [ObservableProperty]
    private DateTime _entryDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan _entryTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    private DiaperTypeOption _selectedType;

    [ObservableProperty]
    private string _notes = "";

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private bool _isSaving;

    private DateTime _createdAt;

    public event Action? Completed;

    public DiaperEntryViewModel(EntryRepository<DiaperEntry> repository, CurrentChildContext childContext)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _childContext = childContext;

        var loc = LocalizationResourceManager.Instance;
        TypeOptions =
        [
            new(DiaperType.Wet, loc["Diaper_Type_Wet"]),
            new(DiaperType.Dirty, loc["Diaper_Type_Dirty"]),
            new(DiaperType.Both, loc["Diaper_Type_Both"]),
        ];
        _selectedType = TypeOptions[0];
    }

    public void LoadEntry(DiaperEntry? entry)
    {
        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            EntryDate = DateTime.Today;
            EntryTime = DateTime.Now.TimeOfDay;
            SelectedType = TypeOptions[0];
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        IsEditing = true;
        EntryDate = entry.OccurredAt.Date;
        EntryTime = entry.OccurredAt.TimeOfDay;
        SelectedType = TypeOptions.First(o => o.Value == entry.Type);
        Notes = entry.Notes ?? "";
        _createdAt = entry.CreatedAt;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_childContext.ChildId is not { } childId) return;

        IsSaving = true;
        try
        {
            var occurredAt = EntryDate.Date + EntryTime;

            if (_entryId is { } id)
            {
                await _repository.UpdateAsync(new DiaperEntry
                {
                    Id = id,
                    ChildId = childId,
                    OccurredAt = occurredAt,
                    Type = SelectedType.Value,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
                    CreatedAt = _createdAt
                });
            }
            else
            {
                await _repository.AddAsync(new DiaperEntry
                {
                    ChildId = childId,
                    OccurredAt = occurredAt,
                    Type = SelectedType.Value,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
                });
            }
            await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Saved"]);
            Completed?.Invoke();
        }
        finally
        {
            IsSaving = false;
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (_entryId is { } id)
        {
            await _repository.DeleteAsync(id);
        }
        await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Deleted"]);
        Completed?.Invoke();
    }
}