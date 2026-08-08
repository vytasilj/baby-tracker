using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BabyTracker.Data;
using BabyTracker.App.Services;

namespace BabyTracker.App.ViewModels;

public record ChildListItem(Guid Id, string Name, bool IsCurrent);

public partial class ChildrenViewModel(ChildRepository repository, CurrentChildContext childContext, ChildDeletionService deletionService) : ObservableObject
{
    public ObservableCollection<ChildListItem> Children { get; } = [];

    public event Action? AddRequested;
    public event Action<Guid>? EditRequested;
    public event Action? SwitchedChild;

    [RelayCommand]
    private async Task Load()
    {
        var all = await repository.GetAllAsync();
        Children.Clear();
        foreach (var c in all)
        {
            Children.Add(new ChildListItem(c.Id, c.Name, c.Id == childContext.ChildId));
        }
    }

    [RelayCommand]
    private void Switch(ChildListItem item)
    {
        childContext.Set(item.Id, item.Name);
        SwitchedChild?.Invoke();
    }

    [RelayCommand]
    private void Edit(ChildListItem item) => EditRequested?.Invoke(item.Id);

    [RelayCommand]
    private void AddNew() => AddRequested?.Invoke();

    public async Task<bool> DeleteConfirmedAsync(ChildListItem item)
    {
        var hasRemaining = await deletionService.DeleteAsync(item.Id);
        await Load();
        return hasRemaining;
    }
}