using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class PumpingListPage : ContentPage
{
    private readonly PumpingListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public PumpingListPage(PumpingListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(PumpingEntry? entry)
    {
        var page = _services.GetRequiredService<PumpingEntryPage>();
        page.LoadEntry(entry);
        await Navigation.PushAsync(page);
    }
}