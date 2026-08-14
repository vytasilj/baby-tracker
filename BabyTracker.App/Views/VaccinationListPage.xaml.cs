using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class VaccinationListPage : ContentPage
{
    private readonly VaccinationListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public VaccinationListPage(VaccinationListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(VaccinationEntry? entry)
    {
        var page = _services.GetRequiredService<VaccinationEntryPage>();
        await page.LoadEntryAsync(entry);
        await Navigation.PushAsync(page);
    }
}