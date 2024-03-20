namespace MilkrunApi.Models;

/// <summary>
///     Read model representing a subset of available products
/// </summary>
/// <param name="products">List of products</param>
/// <param name="total">Total number of products in the system</param>
/// <param name="skip">The skip (page) of the request</param>
/// <param name="limit">The limit (page size) of the request</param>
public class ProductsCollection(IEnumerable<ProductResponse> products, int total, int skip, int limit)
{
    /// <summary>
    ///     List of products
    /// </summary>
    public IEnumerable<ProductResponse> Products { get; } = products;

    /// <summary>
    ///     Total number of products available
    /// </summary>
    public int Total { get; } = total;

    /// <summary>
    ///     The page (skip) of the request
    /// </summary>
    public int Skip { get; } = skip;

    /// <summary>
    ///     The page size (limit) of the request
    /// </summary>
    public int Limit { get; } = limit;
}