using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class TemperatureEntryPage : ContentPage
{
    private readonly TemperatureEntryViewModel _viewModel;

    public TemperatureEntryPage(TemperatureEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public void LoadEntry(TemperatureEntry? entry) => _viewModel.LoadEntry(entry);
}