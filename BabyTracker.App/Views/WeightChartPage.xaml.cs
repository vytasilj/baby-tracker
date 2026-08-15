using BabyTracker.App.ViewModels;

namespace BabyTracker.App.Views;

public partial class WeightChartPage : ContentPage
{
    private readonly WeightChartViewModel _viewModel;

    public WeightChartPage(WeightChartViewModel viewModel)
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