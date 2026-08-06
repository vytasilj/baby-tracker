using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class FeedingListPage : ContentPage
{
    private readonly FeedingListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public FeedingListPage(FeedingListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(FeedingEntry? entry)
    {
        var page = _services.GetRequiredService<FeedingEntryPage>();
        page.LoadEntry(entry);
        await Navigation.PushAsync(page);
    }
}