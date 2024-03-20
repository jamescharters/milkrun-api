using MilkrunApi.Models;
using MilkrunApi.Models.Entity;

namespace MilkrunApi.Services;

public interface IProductsService
{
    Task<ProductsCollection> GetAllAsync(int page = 0, int pageSize = 10);
    Task<ProductEntity> CreateAsync(ProductRequest createProductRequest);
    Task UpdateAsync(long id, ProductRequest updateProductRequest);
}