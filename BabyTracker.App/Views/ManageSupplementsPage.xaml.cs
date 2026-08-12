using BabyTracker.App.ViewModels;

namespace BabyTracker.App.Views;

public partial class ManageSupplementsPage : ContentPage
{
    private readonly ManageSupplementsViewModel _viewModel;

    public ManageSupplementsPage(ManageSupplementsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadCommand.Execute(null);
    }
}