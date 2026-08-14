using BabyTracker.App.ViewModels;
using BabyTracker.Data;

namespace BabyTracker.App.Views;

public partial class CalendarEntryPage : ContentPage
{
    private readonly CalendarEntryViewModel _viewModel;

    public CalendarEntryPage(CalendarEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        viewModel.Completed += async () => await Navigation.PopAsync();
    }

    public void LoadEntry(CalendarEvent? entry) => _viewModel.LoadEntry(entry);
}