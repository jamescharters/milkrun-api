using System.ComponentModel.DataAnnotations;

namespace MilkrunApi.Models.DTO;

/// <summary>
/// Write data transfer object (DTO) for creation / modification of a product
/// </summary>
public record ProductRequest : IProduct
{
    /// <summary>
    /// Title of the product
    /// </summary>
    [DataType(DataType.Text, ErrorMessage = "Title must be text.")]
    [Required(ErrorMessage = "Title is required.")]
    public required string Title { get; set; }
    
    /// <summary>
    /// Brief description of the product
    /// </summary>
    [DataType(DataType.Text, ErrorMessage = "Description must be text.")]
    [StringLength(100, ErrorMessage = "Description must be 100 characters or less.")]
    [Required(ErrorMessage = "Description is required.")]
    public required string Description { get; set; }
    
    /// <summary>
    /// Price of the product
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    [Required(ErrorMessage = "Price is required.")]
    public required int Price { get; set; }

    /// <summary>
    /// Discount percentage of the product, if applicable
    /// </summary>
    public decimal? DiscountPercentage { get; set; }
    
    /// <summary>
    /// Rating of the product, if applicable
    /// </summary>
    public decimal? Rating { get; set; }
    
    /// <summary>
    /// Quantity in stock, if applicable
    /// </summary>
    public int? Stock { get; set; }
    
    /// <summary>
    /// Brand name, if applicable
    /// </summary>
    public string? Brand { get; set; }
    
    /// <summary>
    /// Product category, if applicable
    /// </summary>
    public string? Category { get; set; }
    
    /// <summary>
    /// Thumbnail image URL, if applicable
    /// </summary>
    public string? Thumbnail { get; set; }
    
    /// <summary>
    /// Detailed product images, if applicable
    /// </summary>
    public IList<string>? Images { get; set; }
}