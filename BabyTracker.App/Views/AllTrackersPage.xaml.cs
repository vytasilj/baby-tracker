using BabyTracker.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

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
    {
        Page page = kind switch
        {
            TrackerKind.Feeding => _services.GetRequiredService<FeedingListPage>(),
            TrackerKind.Sleep => _services.GetRequiredService<SleepListPage>(),
            TrackerKind.Diaper => _services.GetRequiredService<DiaperListPage>(),
            TrackerKind.Temperature => _services.GetRequiredService<TemperatureListPage>(),
            TrackerKind.Weight => _services.GetRequiredService<WeightListPage>(),
            TrackerKind.Pumping => _services.GetRequiredService<PumpingListPage>(),
            _ => throw new NotSupportedException()
        };
        await Navigation.PushAsync(page);
    }
}