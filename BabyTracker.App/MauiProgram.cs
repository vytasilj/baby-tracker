using BabyTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BabyTracker.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
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