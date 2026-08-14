using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class CalendarListPage : ContentPage
{
    private readonly CalendarListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public CalendarListPage(CalendarListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(CalendarEvent? entry)
    {
        var page = _services.GetRequiredService<CalendarEntryPage>();
        page.LoadEntry(entry);
        await Navigation.PushAsync(page);
    }
}