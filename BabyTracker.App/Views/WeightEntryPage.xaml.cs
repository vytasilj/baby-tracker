using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class WeightEntryPage : ContentPage
{
    private readonly WeightEntryViewModel _viewModel;

    public WeightEntryPage(WeightEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public void LoadEntry(WeightEntry? entry) => _viewModel.LoadEntry(entry);
}