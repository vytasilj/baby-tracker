using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class SleepListPage : ContentPage
{
    private readonly SleepListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public SleepListPage(SleepListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(SleepEntry? entry)
    {
        var page = _services.GetRequiredService<SleepEntryPage>();
        page.LoadEntry(entry);
        await Navigation.PushAsync(page);
    }
}