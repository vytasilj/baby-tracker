using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private AgeDescription? _ageDescription;

    [ObservableProperty]
    private string _childName = "";

    [ObservableProperty]
    private string _age = "";

    [ObservableProperty]
    private string _themeIcon = ThemeService.GetIconForCurrentTheme();

    public event Action? SettingsRequested;

    [RelayCommand]
    private void OpenDiapers() => DiapersRequested?.Invoke();

    public event Action? DiapersRequested;

    public HomeViewModel(ChildRepository repository)
    {
        _ = LoadAsync(repository);
        LocalizationResourceManager.Instance.PropertyChanged += (_, _) =>
        {
            if (_ageDescription is not null)
            {
                Age = AgeFormatter.Format(_ageDescription);
            }
        };
    }

    private async Task LoadAsync(ChildRepository repository)
    {
        var children = await repository.GetAllAsync();
        var child = children.FirstOrDefault();
        if (child is null) return;

        ChildName = child.Name;
        _ageDescription = AgeCalculator.Calculate(child.BirthDate, DateOnly.FromDateTime(DateTime.Today));
        Age = AgeFormatter.Format(_ageDescription);
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeService.ToggleTheme();
        ThemeIcon = ThemeService.GetIconForCurrentTheme();
    }

    [RelayCommand]
    private void OpenSettings() => SettingsRequested?.Invoke();
}