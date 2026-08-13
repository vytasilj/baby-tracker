using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class MomSleepListPage : ContentPage
{
    private readonly MomSleepListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public MomSleepListPage(MomSleepListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(MomSleepEntry? entry)
    {
        var page = _services.GetRequiredService<MomSleepEntryPage>();
        page.LoadEntry(entry);
        await Navigation.PushAsync(page);
    }
}