using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class DiaperEntryPage : ContentPage
{
    private readonly DiaperEntryViewModel _viewModel;

    public DiaperEntryPage(DiaperEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public void LoadEntry(DiaperEntry? entry) => _viewModel.LoadEntry(entry);
}