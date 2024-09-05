using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        try
        {
            return Ok(await repo.GetProductsAsync(brand, type, sort));
        }
        catch (System.Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        try
        {
            var product = await repo.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }
        catch (System.Exception ex)
        {

            return BadRequest(ex.Message);

        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        try
        {
            repo.AddProduct(product);
            if (await repo.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }

            return BadRequest("Problem Creating Product.");
        }
        catch (System.Exception ex)
        {

            return BadRequest(ex.Message);

        }

    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        try
        {
            if (product.Id != id || !ProductExists(id))
                return BadRequest("Cannot update this product");

            repo.UpdateProduct(product);

            if (await repo.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem updating the product.");

        }
        catch (System.Exception ex)
        {

            return BadRequest(ex.Message);
            ;
        }

    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await repo.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            repo.DeleteProduct(product);

            if (await repo.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem deleting the product.");

        }
        catch (System.Exception ex)
        {

            return BadRequest(ex.Message);
        }

    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandAsync());
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
    }
    private bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }
}
