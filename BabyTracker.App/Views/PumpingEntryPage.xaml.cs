using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class PumpingEntryPage : ContentPage
{
    private readonly PumpingEntryViewModel _viewModel;

    public PumpingEntryPage(PumpingEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public void LoadEntry(PumpingEntry? entry) => _viewModel.LoadEntry(entry);
}