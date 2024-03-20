namespace MilkrunApi.Models;

/// <summary>
///     Read model for a product
/// </summary>
public record ProductResponse : IProduct
{
    /// <summary>
    ///     Unique identifier of the product
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    ///     Title of the product
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    ///     Brief description of the product
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    ///     Price of the product
    /// </summary>
    public required int Price { get; set; }

    /// <summary>
    ///     Discount percentage of the product, if applicable
    /// </summary>
    public decimal? DiscountPercentage { get; set; }

    /// <summary>
    ///     Rating of the product, if applicable
    /// </summary>
    public decimal? Rating { get; set; }

    /// <summary>
    ///     Quantity in stock, if applicable
    /// </summary>
    public int? Stock { get; set; }

    /// <summary>
    ///     Brand name, if applicable
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    ///     Product category, if applicable
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    ///     Thumbnail image URL, if applicable
    /// </summary>
    public string? Thumbnail { get; set; }

    /// <summary>
    ///     Detailed product images, if applicable
    /// </summary>
    public IList<string>? Images { get; set; }
}