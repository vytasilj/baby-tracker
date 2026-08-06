using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class SleepEntryPage : ContentPage
{
    private readonly SleepEntryViewModel _viewModel;

    public SleepEntryPage(SleepEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public void LoadEntry(SleepEntry? entry) => _viewModel.LoadEntry(entry);
}