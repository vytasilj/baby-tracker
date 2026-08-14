using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class MedicalExamEntryPage : ContentPage
{
    private readonly MedicalExamEntryViewModel _viewModel;

    public MedicalExamEntryPage(MedicalExamEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public void LoadEntry(MedicalExamEntry? entry) => _viewModel.LoadEntry(entry);
}