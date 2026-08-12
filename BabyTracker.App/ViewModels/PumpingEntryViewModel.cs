using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class PumpingEntryViewModel : ObservableObject
{
    private readonly EntryRepository<PumpingEntry> _repository;
    private readonly CurrentChildContext _childContext;
    private Guid? _entryId;
    private DateTime _createdAt;

    public List<BreastSideOption> SideOptions { get; }

    [ObservableProperty] private DateTime _entryDate = DateTime.Today;
    [ObservableProperty] private TimeSpan _entryTime = DateTime.Now.TimeOfDay;
    [ObservableProperty] private BreastSideOption _selectedSide;
    [ObservableProperty] private int? _amountMl;
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private bool _isSaving;

    public event Action? Completed;

    public PumpingEntryViewModel(EntryRepository<PumpingEntry> repository, CurrentChildContext childContext)
    {
        _repository = repository;
        _childContext = childContext;

        var loc = LocalizationResourceManager.Instance;
        SideOptions =
        [
            new(BreastSide.Left, loc["Feeding_Side_Left"]),
            new(BreastSide.Right, loc["Feeding_Side_Right"]),
            new(BreastSide.Both, loc["Feeding_Side_Both"]),
        ];
        _selectedSide = SideOptions[2]; // "Both" is the most common default for pumping
    }

    public void LoadEntry(PumpingEntry? entry)
    {
        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            EntryDate = DateTime.Today;
            EntryTime = DateTime.Now.TimeOfDay;
            SelectedSide = SideOptions[2];
            AmountMl = null;
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        _createdAt = entry.CreatedAt;
        IsEditing = true;
        EntryDate = entry.OccurredAt.Date;
        EntryTime = entry.OccurredAt.TimeOfDay;
        SelectedSide = SideOptions.First(o => o.Value == entry.Side);
        AmountMl = entry.AmountMl;
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

            if (_entryId is { } id)
            {
                await _repository.UpdateAsync(new PumpingEntry
                {
                    Id = id,
                    ChildId = childId,
                    CreatedAt = _createdAt,
                    OccurredAt = occurredAt,
                    Side = SelectedSide.Value,
                    AmountMl = AmountMl,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
                });
            }
            else
            {
                await _repository.AddAsync(new PumpingEntry
                {
                    ChildId = childId,
                    OccurredAt = occurredAt,
                    Side = SelectedSide.Value,
                    AmountMl = AmountMl,
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