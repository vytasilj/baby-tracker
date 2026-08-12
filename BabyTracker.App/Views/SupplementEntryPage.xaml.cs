using BabyTracker.App.ViewModels;
using BabyTracker.Data;
using CommunityToolkit.Mvvm.Input;

namespace BabyTracker.App.Views;

public partial class SupplementEntryPage : ContentPage
{
    private readonly SupplementEntryViewModel _viewModel;

    public IAsyncRelayCommand<SelectableSupplement> HideRequestCommand { get; }

    public SupplementEntryPage(SupplementEntryViewModel viewModel)
    {
        HideRequestCommand = new AsyncRelayCommand<SelectableSupplement>(OnHideRequestedAsync);

        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public async Task LoadEntryAsync(SupplementEntry? entry) => await _viewModel.LoadEntryAsync(entry);

    private async Task OnHideRequestedAsync(SelectableSupplement? item)
    {
        if (item is null) return;

        var loc = Localization.LocalizationResourceManager.Instance;
        var confirmed = await DisplayAlertAsync(
            loc["Supplement_HideConfirmTitle"],
            string.Format(loc["Supplement_HideConfirmMessage"], item.DisplayName),
            loc["Common_Yes"],
            loc["Common_No"]);

        if (confirmed)
        {
            await _viewModel.HideSupplementConfirmedAsync(item);
        }
    }
}