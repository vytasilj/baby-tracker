using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [ObservableProperty]
    private string _childName = "";

    [ObservableProperty]
    private string _age = "";

    [ObservableProperty]
    private string _themeIcon = ThemeService.GetIconForCurrentTheme();

    public HomeViewModel(ChildRepository repository)
    {
        _ = LoadAsync(repository);
    }

    private async Task LoadAsync(ChildRepository repository)
    {
        var children = await repository.GetAllAsync();
        var child = children.FirstOrDefault();
        if (child is null) return;

        ChildName = child.Name;
        Age = AgeCalculator.Describe(child.BirthDate, DateOnly.FromDateTime(DateTime.Today));
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeService.ToggleTheme();
        ThemeIcon = ThemeService.GetIconForCurrentTheme();
    }
}