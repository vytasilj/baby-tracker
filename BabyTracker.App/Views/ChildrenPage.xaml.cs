using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace BabyTracker.App.Views;

public partial class ChildrenPage : ContentPage
{
    private readonly ChildrenViewModel _viewModel;
    private readonly IServiceProvider _services;

    public IAsyncRelayCommand<ChildListItem> DeleteRequestCommand { get; }

    public ChildrenPage(ChildrenViewModel viewModel, IServiceProvider services)
    {
        DeleteRequestCommand = new AsyncRelayCommand<ChildListItem>(OnDeleteRequestedAsync);

        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _services = services;

        viewModel.AddRequested += OnAddRequested;
        viewModel.EditRequested += OnEditRequested;
        viewModel.SwitchedChild += async () => await Navigation.PopAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadCommand.Execute(null);
    }

    private async void OnAddRequested()
    {
        var page = _services.GetRequiredService<ChildSetupPage>();
        page.SetAddingAdditionalChild();
        await Navigation.PushAsync(page);
    }

    private async void OnEditRequested(Guid childId)
    {
        var repository = _services.GetRequiredService<ChildRepository>();
        var child = await repository.GetByIdAsync(childId);
        if (child is null) return;

        var page = _services.GetRequiredService<ChildSetupPage>();
        page.SetEditingChild(child);
        await Navigation.PushAsync(page);
    }

    private async Task OnDeleteRequestedAsync(ChildListItem? item)
    {
        if (item is null) return;

        var loc = Localization.LocalizationResourceManager.Instance;
        var confirmed = await DisplayAlertAsync(
            loc["Children_DeleteConfirmTitle"],
            string.Format(loc["Children_DeleteConfirmMessage"], item.Name),
            loc["Common_Yes"],
            loc["Common_No"]);

        if (!confirmed) return;

        var hasRemaining = await _viewModel.DeleteConfirmedAsync(item);

        if (!hasRemaining)
        {
            var page = _services.GetRequiredService<ChildSetupPage>();
            Application.Current!.Windows[0].Page = new NavigationPage(page);
        }
        // If hasRemaining is true, DeleteConfirmedAsync already refreshed the list — nothing more to do.
    }
}