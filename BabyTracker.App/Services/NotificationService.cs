using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace BabyTracker.App.Services;

public static class NotificationService
{
    public static async Task ShowAsync(string message)
    {
        var toast = Toast.Make(message, ToastDuration.Short, 14);
        await toast.Show();
    }
}