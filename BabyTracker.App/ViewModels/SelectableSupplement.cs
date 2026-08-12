using CommunityToolkit.Mvvm.ComponentModel;
using BabyTracker.Data;

namespace BabyTracker.App.ViewModels;

public partial class SelectableSupplement(SupplementDefinition definition, string displayName, bool isSelected) : ObservableObject
{
    public SupplementDefinition Definition { get; } = definition;
    public string DisplayName { get; } = displayName;

    [ObservableProperty]
    private bool _isSelected = isSelected;
}