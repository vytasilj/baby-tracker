using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Localization;

namespace BabyTracker.App.ViewModels;

public record ManagedSupplementItem(Guid Id, string DisplayName, bool IsHidden);

public partial class ManageSupplementsViewModel(SupplementRepository repository) : ObservableObject
{
    public ObservableCollection<ManagedSupplementItem> Items { get; } = [];

    [RelayCommand]
    private async Task Load()
    {
        var definitions = await repository.GetAllDefinitionsIncludingHiddenAsync();
        Items.Clear();
        foreach (var d in definitions)
        {
            Items.Add(new ManagedSupplementItem(d.Id, SupplementFormatter.DisplayName(d), d.DeletedAt != null));
        }
    }

    [RelayCommand]
    private async Task ToggleHidden(ManagedSupplementItem item)
    {
        if (item.IsHidden)
        {
            await repository.RestoreDefinitionAsync(item.Id);
        }
        else
        {
            await repository.HideDefinitionAsync(item.Id);
        }
        await Load();
    }
}