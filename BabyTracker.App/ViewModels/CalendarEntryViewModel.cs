using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record ChildFilterOption(Guid? ChildId, string Label);

public partial class CalendarEntryViewModel : ObservableObject
{
    private readonly CalendarEventRepository _repository;
    private Guid? _entryId;
    private DateTime _createdAt;

    public List<ChildFilterOption> ChildOptions { get; private set; } = [];

    [ObservableProperty] private string _title = "";
    [ObservableProperty] private DateTime _entryDate = DateTime.Today;
    [ObservableProperty] private TimeSpan _entryTime = new(9, 0, 0);
    [ObservableProperty] private ChildFilterOption _selectedChild = new(null, "");
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private bool _isSaving;

    public event Action? Completed;

    public CalendarEntryViewModel(CalendarEventRepository repository, ChildRepository childRepository)
    {
        _repository = repository;
        _ = LoadChildOptionsAsync(childRepository);
    }

    private async Task LoadChildOptionsAsync(ChildRepository childRepository)
    {
        var loc = LocalizationResourceManager.Instance;
        var children = await childRepository.GetAllAsync();
        ChildOptions = new List<ChildFilterOption> { new(null, loc["Calendar_FamilyWide"]) }
            .Concat(children.Select(c => new ChildFilterOption(c.Id, c.Name)))
            .ToList();
        SelectedChild = ChildOptions[0];
    }

    public void LoadEntry(CalendarEvent? entry)
    {
        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            Title = "";
            EntryDate = DateTime.Today;
            EntryTime = new TimeSpan(9, 0, 0);
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        _createdAt = entry.CreatedAt;
        IsEditing = true;
        Title = entry.Title;
        EntryDate = entry.OccursAt.Date;
        EntryTime = entry.OccursAt.TimeOfDay;
        Notes = entry.Notes ?? "";
        SelectedChild = ChildOptions.FirstOrDefault(o => o.ChildId == entry.ChildId) ?? ChildOptions[0];
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Title)) return;

        IsSaving = true;
        try
        {
            var occursAt = EntryDate.Date + EntryTime;

            if (_entryId is { } id)
            {
                await _repository.UpdateAsync(new CalendarEvent
                {
                    Id = id,
                    CreatedAt = _createdAt,
                    ChildId = SelectedChild.ChildId,
                    Title = Title.Trim(),
                    OccursAt = occursAt,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
                });
            }
            else
            {
                await _repository.AddAsync(new CalendarEvent
                {
                    ChildId = SelectedChild.ChildId,
                    Title = Title.Trim(),
                    OccursAt = occursAt,
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

    [RelayCommand]
    private void AddToDeviceCalendar()
    {
        var occursAt = EntryDate.Date + EntryTime;
        CalendarIntentHelper.AddToDeviceCalendar(Title, occursAt, Notes);
    }
}