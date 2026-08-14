using BabyTracker.App.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BabyTracker.App;

public static class TrackerNavigation
{
    public static Page ResolveListPage(TrackerKind kind, IServiceProvider services) => kind switch
    {
        TrackerKind.Feeding => services.GetRequiredService<FeedingListPage>(),
        TrackerKind.Sleep => services.GetRequiredService<SleepListPage>(),
        TrackerKind.Diaper => services.GetRequiredService<DiaperListPage>(),
        TrackerKind.Temperature => services.GetRequiredService<TemperatureListPage>(),
        TrackerKind.Weight => services.GetRequiredService<WeightListPage>(),
        TrackerKind.Pumping => services.GetRequiredService<PumpingListPage>(),
        TrackerKind.Supplement => services.GetRequiredService<SupplementListPage>(),
        TrackerKind.MomSleep => services.GetRequiredService<MomSleepListPage>(),
        TrackerKind.Calendar => services.GetRequiredService<CalendarListPage>(),
        TrackerKind.Vaccination => services.GetRequiredService<VaccinationListPage>(),
        _ => throw new NotSupportedException()
    };

    public static async Task NavigateToAddNewAsync(TrackerKind kind, INavigation navigation, IServiceProvider services)
    {
        switch (kind)
        {
            case TrackerKind.Feeding:
                var fp = services.GetRequiredService<FeedingEntryPage>();
                fp.LoadEntry(null);
                await navigation.PushAsync(fp);
                break;
            case TrackerKind.Sleep:
                var sp = services.GetRequiredService<SleepEntryPage>();
                sp.LoadEntry(null);
                await navigation.PushAsync(sp);
                break;
            case TrackerKind.Diaper:
                var dp = services.GetRequiredService<DiaperEntryPage>();
                dp.LoadEntry(null);
                await navigation.PushAsync(dp);
                break;
            case TrackerKind.Temperature:
                var tp = services.GetRequiredService<TemperatureEntryPage>();
                tp.LoadEntry(null);
                await navigation.PushAsync(tp);
                break;
            case TrackerKind.Weight:
                var wp = services.GetRequiredService<WeightEntryPage>();
                wp.LoadEntry(null);
                await navigation.PushAsync(wp);
                break;
            case TrackerKind.Pumping:
                var pp = services.GetRequiredService<PumpingEntryPage>();
                pp.LoadEntry(null);
                await navigation.PushAsync(pp);
                break;
            case TrackerKind.Supplement:
                var sup = services.GetRequiredService<SupplementEntryPage>();
                await sup.LoadEntryAsync(null);
                await navigation.PushAsync(sup);
                break;
            case TrackerKind.MomSleep:
                var mp = services.GetRequiredService<MomSleepEntryPage>();
                mp.LoadEntry(null);
                await navigation.PushAsync(mp);
                break;
            case TrackerKind.Calendar:
                var cp = services.GetRequiredService<CalendarEntryPage>();
                cp.LoadEntry(null);
                await navigation.PushAsync(cp);
                break;
            case TrackerKind.Vaccination:
                var vp = services.GetRequiredService<VaccinationEntryPage>();
                await vp.LoadEntryAsync(null);
                await navigation.PushAsync(vp);
                break;
        }
    }
}