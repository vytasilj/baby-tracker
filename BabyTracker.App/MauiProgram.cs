using BabyTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using BabyTracker.App.Extensions;

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

		builder.Services
			.AddAppData()
			.AddAppServices()
			.AddViewModelsAndPages();

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