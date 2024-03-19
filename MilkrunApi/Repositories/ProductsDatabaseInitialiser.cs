using Microsoft.Extensions.Options;
using MilkrunApi.Data;
using MilkrunApi.Models.Entity;
using Newtonsoft.Json;

namespace MilkrunApi.Repositories;

internal class ProductsDatabaseInitialiser
{
    private readonly IOptions<MilkrunApiOptions> _options;

    public ProductsDatabaseInitialiser(IOptions<MilkrunApiOptions> options)
    {
        _options = options;
    }

    internal async Task SeedAsync(ProductsDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

        await dbContext.Database.EnsureCreatedAsync();

        if (dbContext.Products.Any()) return;

        using var sr = new StreamReader(_options.Value.DataSeedJsonPath);

        var json = await sr.ReadToEndAsync();

        var products = JsonConvert.DeserializeObject<IReadOnlyList<ProductEntity>>(json);

        foreach (var product in products!) await dbContext.Products.AddAsync(product);

        await dbContext.SaveChangesAsync();
    }
}