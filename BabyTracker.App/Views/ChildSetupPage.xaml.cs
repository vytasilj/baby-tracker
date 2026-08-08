using BabyTracker.App.ViewModels;
using BabyTracker.App.Services;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace BabyTracker.App.Views;

public partial class ChildSetupPage : ContentPage
{
    private readonly ChildSetupViewModel _viewModel;
    private readonly IServiceProvider _services;

    public IAsyncRelayCommand DeleteRequestCommand { get; }

    public ChildSetupPage(ChildSetupViewModel viewModel, CurrentChildContext childContext, IServiceProvider services)
    {
        DeleteRequestCommand = new AsyncRelayCommand(OnDeleteRequestedAsync);

        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _services = services;
        BirthDatePicker.MaximumDate = DateTime.Today;

        viewModel.Saved += async () =>
        {
            childContext.Set(viewModel.SavedChildId, viewModel.Name.Trim());

            if (viewModel.IsFirstRun)
            {
                await Navigation.PushAsync(services.GetRequiredService<HomePage>());
            }
            else
            {
                await Navigation.PopToRootAsync();
            }
        };
    }

    public void SetAddingAdditionalChild() => _viewModel.IsFirstRun = false;
    public void SetEditingChild(Child child) => _viewModel.LoadChild(child);

    private async Task OnDeleteRequestedAsync()
    {
        var loc = Localization.LocalizationResourceManager.Instance;
        var confirmed = await DisplayAlertAsync(
            loc["Children_DeleteConfirmTitle"],
            string.Format(loc["Children_DeleteConfirmMessage"], _viewModel.Name),
            loc["Common_Yes"],
            loc["Common_No"]);

        if (!confirmed) return;

        var hasRemaining = await _viewModel.DeleteConfirmedAsync();

        if (hasRemaining)
        {
            await Navigation.PopToRootAsync();
        }
        else
        {
            var page = _services.GetRequiredService<ChildSetupPage>();
            Application.Current!.Windows[0].Page = new NavigationPage(page);
        }
    }
}