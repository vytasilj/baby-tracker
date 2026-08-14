namespace BabyTracker.App.Services;

public static class CalendarIntentHelper
{
    public static void AddToDeviceCalendar(string title, DateTime start, string? notes)
    {
#if ANDROID
        var intent = new Android.Content.Intent(Android.Content.Intent.ActionInsert);
        intent.SetData(Android.Provider.CalendarContract.Events.ContentUri);
        intent.PutExtra(Android.Provider.CalendarContract.IEventsColumns.Title, title);
        intent.PutExtra(Android.Provider.CalendarContract.ExtraEventBeginTime, ToUnixMillis(start));
        intent.PutExtra(Android.Provider.CalendarContract.ExtraEventEndTime, ToUnixMillis(start.AddHours(1)));
        if (!string.IsNullOrWhiteSpace(notes))
        {
            intent.PutExtra(Android.Provider.CalendarContract.IEventsColumns.Description, notes);
        }
        intent.SetFlags(Android.Content.ActivityFlags.NewTask);
        Android.App.Application.Context.StartActivity(intent);
#endif
    }

#if ANDROID
    private static long ToUnixMillis(DateTime dateTime) =>
        new DateTimeOffset(dateTime, TimeZoneInfo.Local.GetUtcOffset(dateTime)).ToUnixTimeMilliseconds();
#endif
}