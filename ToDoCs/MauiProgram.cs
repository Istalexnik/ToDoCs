using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoCs.Data;
using ToDoCs.Services;
using ToDoCs.Pages;
using ToDoCs.ViewModels;
using System.IO;
using Microsoft.Maui.Storage;
using ToDoCs.Helpers;
using System.Diagnostics;

namespace ToDoCs;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configure EF Core with SQLite for runtime
        var databasePath = $"Filename={Path.Combine(FileSystem.AppDataDirectory, "ToDoDatabase2.db")}";
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(databasePath));
        Debug.WriteLine($"Database path: {databasePath}");


        // Apply any pending migrations on startup
        ApplyMigrations(builder.Services);

        builder.Services.AddSingleton<ToDoService>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<ICommunityToolkitHotReloadHandler, HotReloadHandler>();
        builder.Services.AddSingletonWithShellRoute<MainPage, MainViewModel>($"//{typeof(MainPage)}");
        builder.Services.AddTransientWithShellRoute<DetailsPage, DetailsViewModel>(nameof(DetailsPage));
        builder.Services.AddTransientWithShellRoute<AddTaskPage, AddTaskViewModel>(nameof(AddTaskPage));

        return builder.Build();
    }

    private static void ApplyMigrations(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate(); // Applies any pending migrations
    }
}
