using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class WeightListPage : ContentPage
{
    private readonly WeightListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public WeightListPage(WeightListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(WeightEntry? entry)
    {
        var page = _services.GetRequiredService<WeightEntryPage>();
        page.LoadEntry(entry);
        await Navigation.PushAsync(page);
    }
}