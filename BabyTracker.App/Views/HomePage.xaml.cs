using BabyTracker.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;

        viewModel.SettingsRequested += async () => await Navigation.PushAsync(services.GetRequiredService<SettingsPage>());
        viewModel.ChildrenRequested += async () => await Navigation.PushAsync(services.GetRequiredService<ChildrenPage>());
        viewModel.AllTrackersRequested += async () => await Navigation.PushAsync(services.GetRequiredService<AllTrackersPage>());
        viewModel.OpenTrackerRequested += async kind => await Navigation.PushAsync(TrackerNavigation.ResolveListPage(kind, services));
        viewModel.AddTrackerRequested += async kind => await TrackerNavigation.NavigateToAddNewAsync(kind, Navigation, services);
        viewModel.StatisticsRequested += async () => await Navigation.PushAsync(services.GetRequiredService<StatisticsPage>());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.RefreshSummaryAsync();
    }
}