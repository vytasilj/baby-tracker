using BabyTracker.App.ViewModels;

namespace BabyTracker.App.Views;

public partial class AllTrackersPage : ContentPage
{
    private readonly IServiceProvider _services;

    public AllTrackersPage(AllTrackersViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _services = services;
        viewModel.TrackerSelected += OnTrackerSelected;
    }

    private async void OnTrackerSelected(TrackerKind kind)
        => await Navigation.PushAsync(TrackerNavigation.ResolveListPage(kind, _services));
}