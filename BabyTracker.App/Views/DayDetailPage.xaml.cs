using BabyTracker.App.ViewModels;

namespace BabyTracker.App.Views;

public partial class DayDetailPage : ContentPage
{
    public DayDetailPage(DayDetailViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.OpenTrackerRequested += async kind => await Navigation.PushAsync(TrackerNavigation.ResolveListPage(kind, services));
    }
}