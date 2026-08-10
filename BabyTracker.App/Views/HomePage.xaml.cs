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
        viewModel.DiapersRequested += async () => await Navigation.PushAsync(services.GetRequiredService<DiaperListPage>());
        viewModel.FeedingRequested += async () => await Navigation.PushAsync(services.GetRequiredService<FeedingListPage>());
        viewModel.SleepRequested += async () => await Navigation.PushAsync(services.GetRequiredService<SleepListPage>());

        viewModel.AddDiaperRequested += async () =>
        {
            var page = services.GetRequiredService<DiaperEntryPage>();
            page.LoadEntry(null);
            await Navigation.PushAsync(page);
        };
        viewModel.AddFeedingRequested += async () =>
        {
            var page = services.GetRequiredService<FeedingEntryPage>();
            page.LoadEntry(null);
            await Navigation.PushAsync(page);
        };
        viewModel.AddSleepRequested += async () =>
        {
            var page = services.GetRequiredService<SleepEntryPage>();
            page.LoadEntry(null);
            await Navigation.PushAsync(page);
        };
        viewModel.AllTrackersRequested += async () => await Navigation.PushAsync(services.GetRequiredService<AllTrackersPage>());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh the daily summary every time we return to Home — e.g. after
        // adding/editing/deleting an entry via one of the "+" buttons below.
        _ = _viewModel.RefreshSummaryAsync();
    }
}