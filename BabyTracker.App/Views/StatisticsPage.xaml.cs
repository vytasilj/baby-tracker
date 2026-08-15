using BabyTracker.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage(StatisticsViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.DayDetailRequested += async () => await Navigation.PushAsync(services.GetRequiredService<DayDetailPage>());
        viewModel.WeightChartRequested += async () => await Navigation.PushAsync(services.GetRequiredService<WeightChartPage>());
    }
}