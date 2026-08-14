using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class MedicalExamListPage : ContentPage
{
    private readonly MedicalExamListViewModel _viewModel;
    private readonly IServiceProvider _services;

    public MedicalExamListPage(MedicalExamListViewModel viewModel, IServiceProvider services)
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

    private async void OnEditRequested(MedicalExamEntry? entry)
    {
        var page = _services.GetRequiredService<MedicalExamEntryPage>();
        page.LoadEntry(entry);
        await Navigation.PushAsync(page);
    }
}