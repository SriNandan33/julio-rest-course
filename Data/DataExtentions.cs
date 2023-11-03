using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

public static class DataExtensions
{
    public static void InitializeDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();

        var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
        .CreateLogger("DB Initializer");

        logger.LogInformation("Database is ready!");
    }
}