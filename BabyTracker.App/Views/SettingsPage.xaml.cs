using BabyTracker.App.ViewModels;

namespace BabyTracker.App.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.ManageSupplementsRequested += async () => await Navigation.PushAsync(services.GetRequiredService<ManageSupplementsPage>());
        viewModel.CustomizeHomeRequested += async () => await Navigation.PushAsync(services.GetRequiredService<CustomizeHomePage>());
    }
}