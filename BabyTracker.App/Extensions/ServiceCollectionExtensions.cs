using BabyTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace BabyTracker.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppData(this IServiceCollection services)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "babytracker.db");
        services.AddDbContextFactory<BabyTrackerDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        services.AddSingleton<ChildRepository>();
        services.AddSingleton<SleepRepository>();
        services.AddSingleton<EntryRepository<FeedingEntry>>();
        services.AddSingleton<EntryRepository<DiaperEntry>>();
        services.AddSingleton<EntryRepository<TemperatureEntry>>();
        services.AddSingleton<EntryRepository<WeightEntry>>();
        services.AddSingleton<EntryRepository<PumpingEntry>>();
        services.AddSingleton<SupplementRepository>();
        services.AddSingleton<MomSleepRepository>();
        services.AddSingleton<CalendarEventRepository>();
        services.AddSingleton<VaccinationRepository>();
        services.AddSingleton<EntryRepository<MedicalExamEntry>>();

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<Services.CurrentChildContext>();
        services.AddSingleton<Services.ChildDeletionService>();
        services.AddSingleton<Services.UnitPreferenceService>();
        services.AddSingleton<Services.HomeLayoutPreferenceService>();
        services.AddSingleton<Services.DailyTrackerSummaryService>();
        return services;
    }

    public static IServiceCollection AddViewModelsAndPages(this IServiceCollection services)
    {
        services.AddTransient<Views.StartupPage>();

        services.AddTransient<ViewModels.ChildSetupViewModel>();
        services.AddTransient<Views.ChildSetupPage>();

        services.AddTransient<ViewModels.HomeViewModel>();
        services.AddTransient<Views.HomePage>();

        services.AddTransient<ViewModels.SettingsViewModel>();
        services.AddTransient<Views.SettingsPage>();

        services.AddTransient<ViewModels.ChildrenViewModel>();
        services.AddTransient<Views.ChildrenPage>();

        services.AddTransient<ViewModels.DiaperListViewModel>();
        services.AddTransient<Views.DiaperListPage>();
        services.AddTransient<ViewModels.DiaperEntryViewModel>();
        services.AddTransient<Views.DiaperEntryPage>();

        services.AddTransient<ViewModels.FeedingListViewModel>();
        services.AddTransient<Views.FeedingListPage>();
        services.AddTransient<ViewModels.FeedingEntryViewModel>();
        services.AddTransient<Views.FeedingEntryPage>();

        services.AddTransient<ViewModels.SleepListViewModel>();
        services.AddTransient<Views.SleepListPage>();
        services.AddTransient<ViewModels.SleepEntryViewModel>();
        services.AddTransient<Views.SleepEntryPage>();

        services.AddTransient<ViewModels.TemperatureListViewModel>();
        services.AddTransient<Views.TemperatureListPage>();
        services.AddTransient<ViewModels.TemperatureEntryViewModel>();
        services.AddTransient<Views.TemperatureEntryPage>();

        services.AddTransient<ViewModels.WeightListViewModel>();
        services.AddTransient<Views.WeightListPage>();
        services.AddTransient<ViewModels.WeightEntryViewModel>();
        services.AddTransient<Views.WeightEntryPage>();

        services.AddTransient<ViewModels.AllTrackersViewModel>();
        services.AddTransient<Views.AllTrackersPage>();

        services.AddTransient<ViewModels.PumpingListViewModel>();
        services.AddTransient<Views.PumpingListPage>();
        services.AddTransient<ViewModels.PumpingEntryViewModel>();
        services.AddTransient<Views.PumpingEntryPage>();

        services.AddTransient<ViewModels.SupplementListViewModel>();
        services.AddTransient<Views.SupplementListPage>();
        services.AddTransient<ViewModels.SupplementEntryViewModel>();
        services.AddTransient<Views.SupplementEntryPage>();

        services.AddTransient<ViewModels.ManageSupplementsViewModel>();
        services.AddTransient<Views.ManageSupplementsPage>();

        services.AddTransient<ViewModels.MomSleepListViewModel>();
        services.AddTransient<Views.MomSleepListPage>();
        services.AddTransient<ViewModels.MomSleepEntryViewModel>();
        services.AddTransient<Views.MomSleepEntryPage>();

        services.AddTransient<ViewModels.CustomizeHomeViewModel>();
        services.AddTransient<Views.CustomizeHomePage>();

        services.AddTransient<ViewModels.CalendarListViewModel>();
        services.AddTransient<Views.CalendarListPage>();
        services.AddTransient<ViewModels.CalendarEntryViewModel>();
        services.AddTransient<Views.CalendarEntryPage>();

        services.AddTransient<ViewModels.VaccinationListViewModel>();
        services.AddTransient<Views.VaccinationListPage>();
        services.AddTransient<ViewModels.VaccinationEntryViewModel>();
        services.AddTransient<Views.VaccinationEntryPage>();

        services.AddTransient<ViewModels.MedicalExamListViewModel>();
        services.AddTransient<Views.MedicalExamListPage>();
        services.AddTransient<ViewModels.MedicalExamEntryViewModel>();
        services.AddTransient<Views.MedicalExamEntryPage>();

        services.AddTransient<ViewModels.StatisticsViewModel>();
        services.AddTransient<Views.StatisticsPage>();
        services.AddTransient<ViewModels.DayDetailViewModel>();
        services.AddTransient<Views.DayDetailPage>();

        return services;
    }
}