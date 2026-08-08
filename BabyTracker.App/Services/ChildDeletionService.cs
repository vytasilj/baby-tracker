using BabyTracker.Data;

namespace BabyTracker.App.Services;

// Single place that knows how to delete a child AND correctly fix up
// CurrentChildContext afterwards — used by both the Children list (trash icon)
// and the Edit screen, so they can't drift out of sync with each other again.
public class ChildDeletionService(ChildRepository repository, CurrentChildContext childContext)
{
    public async Task<bool> DeleteAsync(Guid childId)
    {
        var wasCurrent = childContext.ChildId == childId;

        await repository.DeleteAsync(childId);

        if (!wasCurrent) return true;

        var remaining = await repository.GetAllAsync();
        if (remaining.Count == 0)
        {
            childContext.Clear();
            return false;
        }

        var next = remaining[0];
        childContext.Set(next.Id, next.Name);
        return true;
    }
}