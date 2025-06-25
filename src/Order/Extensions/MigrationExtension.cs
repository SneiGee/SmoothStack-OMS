using Microsoft.EntityFrameworkCore;
using Order.Data;

namespace Order.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using AppDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();

        // Seed data
        SeedData.Initialize(dbContext);
    }
}
