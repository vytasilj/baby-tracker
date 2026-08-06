using BabyTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace BabyTracker.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "babytracker.db");

		builder.Services.AddDbContextFactory<BabyTrackerDbContext>(options =>
			options.UseSqlite($"Data Source={dbPath}"));
		builder.Services.AddSingleton<ChildRepository>();
		builder.Services.AddTransient<Views.StartupPage>();
		builder.Services.AddTransient<ViewModels.ChildSetupViewModel>();
		builder.Services.AddTransient<Views.ChildSetupPage>();
		builder.Services.AddTransient<ViewModels.HomeViewModel>();
		builder.Services.AddTransient<Views.HomePage>();
		builder.Services.AddTransient<ViewModels.SettingsViewModel>();
		builder.Services.AddTransient<Views.SettingsPage>();
		builder.Services.AddSingleton<Services.CurrentChildContext>();
		builder.Services.AddSingleton<DiaperRepository>();
		builder.Services.AddTransient<ViewModels.DiaperListViewModel>();
		builder.Services.AddTransient<Views.DiaperListPage>();
		builder.Services.AddTransient<ViewModels.DiaperEntryViewModel>();
		builder.Services.AddTransient<Views.DiaperEntryPage>();
		builder.Services.AddSingleton<FeedingRepository>();
		builder.Services.AddTransient<ViewModels.FeedingListViewModel>();
		builder.Services.AddTransient<Views.FeedingListPage>();
		builder.Services.AddTransient<ViewModels.FeedingEntryViewModel>();
		builder.Services.AddTransient<Views.FeedingEntryPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();

		using (var scope = app.Services.CreateScope())
		{
			var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<BabyTrackerDbContext>>();
			using var db = dbFactory.CreateDbContext();
			db.Database.Migrate();
		}

		return app;
	}
}