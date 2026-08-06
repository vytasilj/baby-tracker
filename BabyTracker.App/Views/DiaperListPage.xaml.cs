using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class DiaperListPage : ContentPage
{
    private readonly DiaperListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public DiaperListPage(DiaperListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(DiaperEntry? entry)
    {
        var page = _services.GetRequiredService<DiaperEntryPage>();
        page.LoadEntry(entry);
        await Navigation.PushAsync(page);
    }
}