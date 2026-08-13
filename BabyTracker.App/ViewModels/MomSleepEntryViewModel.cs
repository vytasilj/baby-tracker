using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class MomSleepEntryViewModel(MomSleepRepository repository) : ObservableObject
{
    private Guid? _entryId;
    private DateTime _createdAt;

    [ObservableProperty] private DateTime _startDate = DateTime.Today;
    [ObservableProperty] private TimeSpan _startTime = DateTime.Now.TimeOfDay;
    [ObservableProperty] private bool _hasEnded;
    [ObservableProperty] private DateTime _endDate = DateTime.Today;
    [ObservableProperty] private TimeSpan _endTime = DateTime.Now.TimeOfDay;
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private string _errorMessage = "";
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private bool _isSaving;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public event Action? Completed;

    public void LoadEntry(MomSleepEntry? entry)
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
        _createdAt = entry.CreatedAt;
        IsEditing = true;
        StartDate = entry.StartTime.Date;
        StartTime = entry.StartTime.TimeOfDay;
        HasEnded = entry.EndTime is not null;
        EndDate = entry.EndTime?.Date ?? DateTime.Today;
        EndTime = entry.EndTime?.TimeOfDay ?? DateTime.Now.TimeOfDay;
        Notes = entry.Notes ?? "";
    }

    [RelayCommand] private void SetStartNow() { StartDate = DateTime.Today; StartTime = DateTime.Now.TimeOfDay; }
    [RelayCommand] private void SetEndNow() { EndDate = DateTime.Today; EndTime = DateTime.Now.TimeOfDay; }

    [RelayCommand]
    private async Task Save()
    {
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
                await repository.UpdateAsync(new MomSleepEntry
                {
                    Id = id,
                    CreatedAt = _createdAt,
                    StartTime = start,
                    EndTime = end,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
                });
            }
            else
            {
                await repository.AddAsync(new MomSleepEntry
                {
                    StartTime = start,
                    EndTime = end,
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