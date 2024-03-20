using System.ComponentModel.DataAnnotations;

namespace MilkrunApi.Models;

/// <summary>
///     Write model for creation / modification of a product
/// </summary>
public record ProductRequest : IProduct
{
    /// <summary>
    ///     Title of the product
    /// </summary>
    [DataType(DataType.Text, ErrorMessage = "Title must be text.")]
    [Required(ErrorMessage = "Title is required.")]
    public required string Title { get; set; }

    /// <summary>
    ///     Brief description of the product
    /// </summary>
    [DataType(DataType.Text, ErrorMessage = "Description must be text.")]
    [StringLength(100, ErrorMessage = "Description must be 100 characters or less.")]
    [Required(ErrorMessage = "Description is required.")]
    public required string Description { get; set; }

    /// <summary>
    ///     Price of the product
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    [Required(ErrorMessage = "Price is required.")]
    [RegularExpression("^(0|[1-9]\\d*)$", ErrorMessage = "Price must be integer >= 0.")]
    public required int Price { get; set; }

    /// <summary>
    ///     Discount percentage of the product, if applicable
    /// </summary>
    [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
    [RegularExpression("^\\d+(\\.\\d{1,2})?$", ErrorMessage = "Discount percentage must be decimal up to 2 places.")]
    public decimal? DiscountPercentage { get; set; }

    /// <summary>
    ///     Rating of the product, if applicable
    /// </summary>
    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
    [RegularExpression("^\\d+(\\.\\d{1,2})?$", ErrorMessage = "Rating must be decimal up to 2 places.")]
    public decimal? Rating { get; set; }

    /// <summary>
    ///     Quantity in stock, if applicable
    /// </summary>
    [RegularExpression("^(0|[1-9]\\d*)$", ErrorMessage = "Stock must be integer >= 0.")]
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