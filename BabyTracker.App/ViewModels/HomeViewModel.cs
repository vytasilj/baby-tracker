using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly ChildRepository _repository;
    private readonly CurrentChildContext _childContext;
    private AgeDescription? _ageDescription;

    [ObservableProperty]
    private string _childName = "";

    [ObservableProperty]
    private string _age = "";

    [ObservableProperty]
    private string _themeIcon = ThemeService.GetIconForCurrentTheme();

    public event Action? SettingsRequested;
    public event Action? ChildrenRequested;
    public event Action? DiapersRequested;
    public event Action? FeedingRequested;
    public event Action? SleepRequested;

    public HomeViewModel(ChildRepository repository, CurrentChildContext childContext)
    {
        _repository = repository;
        _childContext = childContext;

        _ = RefreshAsync();
        _childContext.Changed += async () => await RefreshAsync();

        LocalizationResourceManager.Instance.PropertyChanged += (_, _) =>
        {
            if (_ageDescription is not null) Age = AgeFormatter.Format(_ageDescription);
        };
    }

    private async Task RefreshAsync()
    {
        if (_childContext.ChildId is not { } childId) return;

        var child = await _repository.GetByIdAsync(childId);
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

    [RelayCommand] private void OpenSettings() => SettingsRequested?.Invoke();
    [RelayCommand] private void OpenChildren() => ChildrenRequested?.Invoke();
    [RelayCommand] private void OpenDiapers() => DiapersRequested?.Invoke();
    [RelayCommand] private void OpenFeeding() => FeedingRequested?.Invoke();
    [RelayCommand] private void OpenSleep() => SleepRequested?.Invoke();
}