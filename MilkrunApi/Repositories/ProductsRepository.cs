using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MilkrunApi.Data;
using MilkrunApi.Exceptions;
using MilkrunApi.Models.DTO;
using MilkrunApi.Models.Entity;

namespace MilkrunApi.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly ProductsDbContext _productsDbContext;
    private readonly IMapper _mapper;

    public ProductsRepository(ProductsDbContext dbContext, IMapper mapper)
    {
        _productsDbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<bool> ExistsAsync(string title, string brand)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(brand);
        
        return await _productsDbContext.Products.AnyAsync(product =>
            product.Title.Equals(title) && product.Brand.Equals(brand));
    }

    public async Task<Tuple<IEnumerable<ProductEntity>, int>> GetAllAsync(int page = 0, int pageSize = 10)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(page);
        ArgumentOutOfRangeException.ThrowIfNegative(pageSize);

        var queryResults = await _productsDbContext.Products.OrderBy(p => p.Id).Skip(page * pageSize).Take(pageSize)
            .ToListAsync();

        var totalCount = await _productsDbContext.Products.CountAsync();
        
        return new Tuple<IEnumerable<ProductEntity>, int>(queryResults, totalCount);
    }

    public async Task<ProductEntity> CreateAsync(ProductRequest createProductRequest)
    {
        var result = await _productsDbContext.Products.AddAsync(_mapper.Map<ProductEntity>(createProductRequest));

        await _productsDbContext.SaveChangesAsync();

        return result.Entity;
    }

    public async Task UpdateAsync(long id, ProductRequest updateProductRequest)
    {
        var existingProduct = await _productsDbContext.Products.FindAsync(id);

        if (existingProduct == null)
        {
            return;
        }

        _mapper.Map(updateProductRequest, existingProduct);

        _productsDbContext.Products.Update(existingProduct);

        await _productsDbContext.SaveChangesAsync();
    }
}