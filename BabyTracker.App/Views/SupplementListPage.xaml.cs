using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class SupplementListPage : ContentPage
{
    private readonly SupplementListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public SupplementListPage(SupplementListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(SupplementEntry? entry)
    {
        var page = _services.GetRequiredService<SupplementEntryPage>();
        await page.LoadEntryAsync(entry);
        await Navigation.PushAsync(page);
    }
}