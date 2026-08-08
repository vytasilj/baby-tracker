using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record FeedingTypeOption(FeedingType Value, string Display);
public record BreastSideOption(BreastSide Value, string Display);

public partial class FeedingEntryViewModel : ObservableObject
{
    private readonly FeedingRepository _repository;
    private readonly CurrentChildContext _childContext;
    private Guid? _entryId;

    public List<FeedingTypeOption> TypeOptions { get; }
    public List<BreastSideOption> SideOptions { get; }

    [ObservableProperty]
    private DateTime _entryDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan _entryTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowSideField))]
    [NotifyPropertyChangedFor(nameof(ShowAmountField))]
    private FeedingTypeOption _selectedType;

    [ObservableProperty]
    private BreastSideOption _selectedSide;

    [ObservableProperty]
    private int? _amountMl;

    [ObservableProperty]
    private string _notes = "";

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private bool _isSaving;

    public bool ShowSideField => SelectedType.Value == FeedingType.Breast;
    public bool ShowAmountField => SelectedType.Value == FeedingType.Bottle;

    private DateTime _createdAt;

    public event Action? Completed;

    public FeedingEntryViewModel(FeedingRepository repository, CurrentChildContext childContext)
    {
        _repository = repository;
        _childContext = childContext;

        var loc = LocalizationResourceManager.Instance;
        TypeOptions =
        [
            new(FeedingType.Breast, loc["Feeding_Type_Breast"]),
            new(FeedingType.Bottle, loc["Feeding_Type_Bottle"]),
            new(FeedingType.Solid, loc["Feeding_Type_Solid"]),
        ];
        SideOptions =
        [
            new(BreastSide.Left, loc["Feeding_Side_Left"]),
            new(BreastSide.Right, loc["Feeding_Side_Right"]),
            new(BreastSide.Both, loc["Feeding_Side_Both"]),
        ];

        _selectedType = TypeOptions[0];
        _selectedSide = SideOptions[0];
    }

    public void LoadEntry(FeedingEntry? entry)
    {
        if (entry is null)
        {
            _entryId = null;
            IsEditing = false;
            EntryDate = DateTime.Today;
            EntryTime = DateTime.Now.TimeOfDay;
            SelectedType = TypeOptions[0];
            SelectedSide = SideOptions[0];
            AmountMl = null;
            Notes = "";
            return;
        }

        _entryId = entry.Id;
        IsEditing = true;
        EntryDate = entry.OccurredAt.Date;
        EntryTime = entry.OccurredAt.TimeOfDay;
        SelectedType = TypeOptions.First(o => o.Value == entry.Type);
        SelectedSide = entry.Side is { } side ? SideOptions.First(o => o.Value == side) : SideOptions[0];
        AmountMl = entry.AmountMl;
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
            var side = ShowSideField ? SelectedSide.Value : (BreastSide?)null;
            var amount = ShowAmountField ? AmountMl : null;

            if (_entryId is { } id)
            {
                await _repository.UpdateAsync(new FeedingEntry
                {
                    Id = id,
                    ChildId = childId,
                    OccurredAt = occurredAt,
                    Type = SelectedType.Value,
                    Side = side,
                    AmountMl = amount,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
                    CreatedAt = _createdAt
                });
            }
            else
            {
                await _repository.AddAsync(new FeedingEntry
                {
                    ChildId = childId,
                    OccurredAt = occurredAt,
                    Type = SelectedType.Value,
                    Side = side,
                    AmountMl = amount,
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