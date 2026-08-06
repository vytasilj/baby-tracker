using BabyTracker.Data;
using BabyTracker.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App.Views;

public partial class StartupPage : ContentPage
{
    private readonly ChildRepository _repository;
    private readonly CurrentChildContext _childContext;
    private readonly IServiceProvider _services;
    private bool _hasNavigated;

    public StartupPage(ChildRepository repository, CurrentChildContext childContext, IServiceProvider services)
    {
        InitializeComponent();
        _repository = repository;
        _childContext = childContext;
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
            var child = children.FirstOrDefault();

            Page next;
            if (child is null)
            {
                next = _services.GetRequiredService<ChildSetupPage>();
            }
            else
            {
                _childContext.Set(child.Id, child.Name);
                next = _services.GetRequiredService<HomePage>();
            }

            Navigation.InsertPageBefore(next, this);
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                Localization.LocalizationResourceManager.Instance["Startup_Error_Title"],
                ex.ToString(),
                Localization.LocalizationResourceManager.Instance["Common_Ok"]);
        }
    }
}