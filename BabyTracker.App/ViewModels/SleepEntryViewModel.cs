using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class SleepEntryViewModel : ObservableObject
{
    private readonly SleepRepository _repository;
    private readonly CurrentChildContext _childContext;
    private Guid? _entryId;

    [ObservableProperty]
    private DateTime _startDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan _startTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    private bool _hasEnded;

    [ObservableProperty]
    private DateTime _endDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan _endTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    private string _notes = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = "";

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private bool _isSaving;

    private DateTime _createdAt;

    public event Action? Completed;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public SleepEntryViewModel(SleepRepository repository, CurrentChildContext childContext)
    {
        _repository = repository;
        _childContext = childContext;
    }

    public void LoadEntry(SleepEntry? entry)
    {
        ErrorMessage = "";

        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            StartDate = DateTime.Today;
            StartTime = DateTime.Now.TimeOfDay;
            HasEnded = false;
            EndDate = DateTime.Today;
            EndTime = DateTime.Now.TimeOfDay;
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        IsEditing = true;
        StartDate = entry.StartTime.Date;
        StartTime = entry.StartTime.TimeOfDay;
        HasEnded = entry.EndTime is not null;
        EndDate = entry.EndTime?.Date ?? DateTime.Today;
        EndTime = entry.EndTime?.TimeOfDay ?? DateTime.Now.TimeOfDay;
        Notes = entry.Notes ?? "";
        _createdAt = entry.CreatedAt;
    }

    [RelayCommand]
    private void SetStartNow()
    {
        StartDate = DateTime.Today;
        StartTime = DateTime.Now.TimeOfDay;
    }

    [RelayCommand]
    private void SetEndNow()
    {
        EndDate = DateTime.Today;
        EndTime = DateTime.Now.TimeOfDay;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_childContext.ChildId is not { } childId) return;

        ErrorMessage = "";
        var start = StartDate.Date + StartTime;
        DateTime? end = HasEnded ? EndDate.Date + EndTime : null;

        if (end is not null && end <= start)
        {
            ErrorMessage = LocalizationResourceManager.Instance["Sleep_Error_EndBeforeStart"];
            return;
        }

        IsSaving = true;
        try
        {
            if (_entryId is { } id)
            {
                await _repository.UpdateAsync(new SleepEntry
                {
                    Id = id,
                    ChildId = childId,
                    StartTime = start,
                    EndTime = end,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
                    CreatedAt = _createdAt
                });
            }
            else
            {
                await _repository.AddAsync(new SleepEntry
                {
                    ChildId = childId,
                    StartTime = start,
                    EndTime = end,
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
        if (_entryId is { } id) await _repository.DeleteAsync(id);
        await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Deleted"]);
        Completed?.Invoke();
    }
}