using System.ComponentModel.DataAnnotations;

namespace MilkrunApi.Models.Entity;

/// <summary>
///     Represents the raw data model of a product
/// </summary>
public record ProductEntity : IProduct
{
    [Key] public long Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? Rating { get; set; }
    public int? Stock { get; set; }
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public string? Thumbnail { get; set; }
    public IList<string>? Images { get; set; }
}