using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class FeedingEntryPage : ContentPage
{
    private readonly FeedingEntryViewModel _viewModel;

    public FeedingEntryPage(FeedingEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public void LoadEntry(FeedingEntry? entry) => _viewModel.LoadEntry(entry);
}