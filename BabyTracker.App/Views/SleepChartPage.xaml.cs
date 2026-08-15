using BabyTracker.App.ViewModels;

namespace BabyTracker.App.Views;

public partial class SleepChartPage : ContentPage
{
    private readonly SleepChartViewModel _viewModel;

    public SleepChartPage(SleepChartViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.RefreshAsync();
    }
}