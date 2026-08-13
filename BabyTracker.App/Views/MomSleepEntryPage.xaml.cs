using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class MomSleepEntryPage : ContentPage
{
    private readonly MomSleepEntryViewModel _viewModel;

    public MomSleepEntryPage(MomSleepEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public void LoadEntry(MomSleepEntry? entry) => _viewModel.LoadEntry(entry);
}