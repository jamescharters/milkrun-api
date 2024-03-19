using MilkrunApi.Repositories;

namespace MilkrunApi.Data;

internal static class DbInitializerExtension
{
    public static async Task SeedDatabase(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        using var scope = app.ApplicationServices.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

        await scope.ServiceProvider.GetRequiredService<ProductsDatabaseInitialiser>().SeedAsync(context);
    }
}