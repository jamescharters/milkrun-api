using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MilkrunApi.Exceptions;
using MilkrunApi.Models.DTO;
using MilkrunApi.Models.Entity;
using MilkrunApi.Services;

namespace MilkrunApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class Products(IProductsService productsService) : ControllerBase
{
    /// <summary>
    ///     Retrieve subset of products
    /// </summary>
    /// <param name="page">The page to select</param>
    /// <param name="limit">The number of results per page</param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductsCollection>> GetProducts(
        [Range(0, int.MaxValue, ErrorMessage = $"{nameof(page)} must be >= 0")]
        int page = 0,
        [Range(1, int.MaxValue, ErrorMessage = $"{nameof(limit)} must be >= 1")]
        int limit = 10)
    {
        var results = await productsService.GetAllAsync(page, limit);

        return Ok(results);
    }

    /// <summary>
    ///     Create a new product
    /// </summary>
    /// <param name="createProductRequest">The details of the product to create</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductEntity>> CreateProduct(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)]
        ProductRequest createProductRequest)
    {
        try
        {
            var result = await productsService.CreateAsync(createProductRequest);

            return Created($"api/v1/products/{result.Id}", result);
        }
        catch (DuplicateProductException e)
        {
            return Conflict();
        }
    }

    /// <summary>
    ///     Update an existing product
    /// </summary>
    /// <param name="id">The id of the existing product</param>
    /// <param name="updatedProduct">The updated details of the product</param>
    /// <returns></returns>
    [HttpPut("{id:long}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateProduct(
        [Range(0, long.MaxValue, ErrorMessage = $"{nameof(id)} must be >= 0")] int id,
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)]
        ProductRequest updatedProduct)
    {
        try
        {
            await productsService.UpdateAsync(id, updatedProduct);

            return NoContent();
        }
        catch (DuplicateProductException e)
        {
            return Conflict();
        }
        catch (InvalidProductException e)
        {
            return NotFound();
        }
    }
}