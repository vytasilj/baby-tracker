using BabyTracker.App.ViewModels;

namespace BabyTracker.App.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.SettingsRequested += async () => await Navigation.PushAsync(services.GetRequiredService<SettingsPage>());
        viewModel.DiapersRequested += async () => await Navigation.PushAsync(services.GetRequiredService<DiaperListPage>());
        viewModel.FeedingsRequested += async () => await Navigation.PushAsync(services.GetRequiredService<FeedingListPage>());
    }
}