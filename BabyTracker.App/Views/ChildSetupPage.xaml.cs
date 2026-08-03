using BabyTracker.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class ChildSetupPage : ContentPage
{
    public ChildSetupPage(ChildSetupViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = viewModel;
        BirthDatePicker.MaximumDate = DateTime.Today;
        viewModel.Saved += async () => await Navigation.PushAsync(services.GetRequiredService<HomePage>());
    }
}