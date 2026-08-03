using BabyTracker.App.ViewModels;
using BabyTracker.App.Services;
namespace BabyTracker.App.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}