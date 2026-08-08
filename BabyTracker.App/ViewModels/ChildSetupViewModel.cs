using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public partial class ChildSetupViewModel(ChildRepository repository, ChildDeletionService deletionService) : ObservableObject
{
    private Guid? _childId;
    private DateTime _createdAt;

    [ObservableProperty] private string _name = "";
    [ObservableProperty] private DateTime _birthDate = DateTime.Today;
    [ObservableProperty] private bool _isSaving;
    [ObservableProperty] private bool _isEditing;

    public Guid SavedChildId { get; private set; }
    public bool IsFirstRun { get; set; } = true;

    public event Action? Saved;

    public void LoadChild(Child? child)
    {
        if (child is null)
        {
            _childId = null;
            IsEditing = false;
            Name = "";
            BirthDate = DateTime.Today;
            return;
        }

        _childId = child.Id;
        _createdAt = child.CreatedAt;
        IsEditing = true;
        IsFirstRun = false;
        Name = child.Name;
        BirthDate = child.BirthDate.ToDateTime(TimeOnly.MinValue);
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Name)) return;

        IsSaving = true;
        try
        {
            if (_childId is { } id)
            {
                await repository.UpdateAsync(new Child
                {
                    Id = id,
                    CreatedAt = _createdAt,
                    Name = Name.Trim(),
                    BirthDate = DateOnly.FromDateTime(BirthDate)
                });
                SavedChildId = id;
            }
            else
            {
                var child = await repository.AddAsync(Name.Trim(), DateOnly.FromDateTime(BirthDate));
                SavedChildId = child.Id;
            }

            await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Saved"]);
            Saved?.Invoke();
        }
        finally { IsSaving = false; }
    }

    // Not a [RelayCommand] on purpose — deleting needs a confirmation dialog first,
    // and a ViewModel shouldn't own UI dialogs. The page calls this after confirming.
    public async Task<bool> DeleteConfirmedAsync()
    {
        if (_childId is not { } id) return true;

        var hasRemaining = await deletionService.DeleteAsync(id);
        await NotificationService.ShowAsync(LocalizationResourceManager.Instance["Common_Deleted"]);
        return hasRemaining;
    }
}