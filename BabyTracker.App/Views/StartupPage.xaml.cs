using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class StartupPage : ContentPage
{
    private readonly ChildRepository _repository;
    private readonly IServiceProvider _services;
    private bool _hasNavigated;

    public StartupPage(ChildRepository repository, IServiceProvider services)
    {
        InitializeComponent();
        _repository = repository;
        _services = services;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_hasNavigated) return;
        _hasNavigated = true;

        try
        {
            var children = await _repository.GetAllAsync();
            var next = children.Count == 0
                ? _services.GetRequiredService<ChildSetupPage>()
                : (Page)_services.GetRequiredService<HomePage>();

            Navigation.InsertPageBefore(next, this);
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Startup error", ex.ToString(), "OK");
        }
    }
}