using EFCoreSQLServer.Models;
using EFCoreSQLServer.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreSQLServer.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepositoy _productRepository;

    public ProductsController(IProductRepositoy productRepositoy)
    {
        _productRepository = productRepositoy;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Products()
    {
        return Ok(await _productRepository.GetProducts());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> Product(int id)
    {
        var product = await _productRepository.GetProductById(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _productRepository.CreateProduct(product);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Update(int id, [FromBody] Product productFromJson)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = await _productRepository.UpdateProduct(id, productFromJson);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> Delete(int id)
    {
        var product = await _productRepository.DeleteProduct(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }
}
