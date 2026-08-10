using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class TemperatureListPage : ContentPage
{
    private readonly TemperatureListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public TemperatureListPage(TemperatureListViewModel viewModel, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _services = services;
        viewModel.EditRequested += OnEditRequested;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadCommand.Execute(null);
    }

    private async void OnEditRequested(TemperatureEntry? entry)
    {
        var page = _services.GetRequiredService<TemperatureEntryPage>();
        page.LoadEntry(entry);
        await Navigation.PushAsync(page);
    }
}