using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class VaccinationEntryPage : ContentPage
{
    private readonly VaccinationEntryViewModel _viewModel;

    public VaccinationEntryPage(VaccinationEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public async Task LoadEntryAsync(VaccinationEntry? entry) => await _viewModel.LoadEntryAsync(entry);
}