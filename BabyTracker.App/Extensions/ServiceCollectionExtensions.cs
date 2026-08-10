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

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<Services.CurrentChildContext>();
        services.AddSingleton<Services.ChildDeletionService>();
        services.AddSingleton<Services.UnitPreferenceService>();
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

        return services;
    }
}