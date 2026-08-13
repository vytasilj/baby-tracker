using BabyTracker.App.ViewModels;

namespace BabyTracker.App.Views;

public partial class CustomizeHomePage : ContentPage
{
    public CustomizeHomePage(CustomizeHomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}