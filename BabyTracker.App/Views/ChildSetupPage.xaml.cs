using BabyTracker.App.ViewModels;
using BabyTracker.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class ChildSetupPage : ContentPage
{
    public ChildSetupPage(ChildSetupViewModel viewModel, CurrentChildContext childContext, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = viewModel;
        BirthDatePicker.MaximumDate = DateTime.Today;
        viewModel.Saved += async () =>
        {
            childContext.Set(viewModel.SavedChildId, viewModel.Name.Trim());
            await Navigation.PushAsync(services.GetRequiredService<HomePage>());
        };
    }
}