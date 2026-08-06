using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;

namespace BabyTracker.App.ViewModels;

public partial class ChildSetupViewModel(ChildRepository repository) : ObservableObject
{
    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private DateTime _birthDate = DateTime.Today;

    [ObservableProperty]
    private bool _isSaving;

    public Guid SavedChildId { get; private set; }

    public event Action? Saved;

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Name)) return;

        IsSaving = true;
        try
        {
            var child = await repository.AddAsync(Name.Trim(), DateOnly.FromDateTime(BirthDate));
            SavedChildId = child.Id;
            Saved?.Invoke();
        }
        finally
        {
            IsSaving = false;
        }
    }
}