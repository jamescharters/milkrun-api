using Microsoft.EntityFrameworkCore;
using MilkrunApi.Models.Entity;

namespace MilkrunApi.Data;

public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>(entity => entity.ToTable("Products"));

        modelBuilder.Entity<ProductEntity>().Property(entity => entity.Id)
            .ValueGeneratedOnAdd();
    }
}