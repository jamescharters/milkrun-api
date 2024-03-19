using MilkrunApi.Models.DTO;
using MilkrunApi.Models.Entity;

namespace MilkrunApi.Repositories;

public interface IProductsRepository
{
    Task<bool> ExistsAsync(string title, string brand);
    Task<Tuple<IEnumerable<ProductEntity>, int>> GetAllAsync(int page = 0, int pageSize = 10);
    Task<ProductEntity> CreateAsync(ProductRequest createProductRequest);
    Task UpdateAsync(long id, ProductRequest updateProductRequest);
}