using AutoMapper;
using MilkrunApi.Exceptions;
using MilkrunApi.Models.DTO;
using MilkrunApi.Models.Entity;
using MilkrunApi.Repositories;

namespace MilkrunApi.Services;

public class ProductsService(IProductsRepository productsRepository, IMapper mapper) : IProductsService
{
    public async Task<ProductsCollection> GetAllAsync(int page = 0, int pageSize = 10)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(page);
        ArgumentOutOfRangeException.ThrowIfNegative(pageSize);

        var results = await productsRepository.GetAllAsync(page, pageSize);

        var mappedResults = mapper.Map<IEnumerable<ProductEntity>, IEnumerable<ProductResponse>>(results.Item1);

        return new ProductsCollection(mappedResults, results.Item2, page, pageSize);
    }

    public async Task<ProductEntity> CreateAsync(ProductRequest createProductRequest)
    {
        var productExists =
            await productsRepository.ExistsAsync(createProductRequest.Title, createProductRequest.Brand);

        if (productExists)
            throw new DuplicateProductException(
                $"Product with title '{createProductRequest.Title}' and brand '{createProductRequest.Brand}' already exists");

        return await productsRepository.CreateAsync(createProductRequest);
    }

    public async Task UpdateAsync(long id, ProductRequest updateProductRequest)
    {
        var existingProduct = await productsRepository.ExistsAsync(id);

        if (!existingProduct) throw new InvalidProductException($"Product with id '{id}' does not exist");

        var duplicateProduct =
            await productsRepository.ExistsAsync(updateProductRequest.Title, updateProductRequest.Brand);

        if (duplicateProduct)
            throw new DuplicateProductException(
                $"Product with title '{updateProductRequest.Title}' and brand '{updateProductRequest.Brand}' already exists");

        await productsRepository.UpdateAsync(id, updateProductRequest);
    }
}