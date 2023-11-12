using EFCoreSQLServer.Context;
using EFCoreSQLServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSQLServer.Repository;

public class ProductRepositoy : IProductRepositoy
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepositoy(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _dbContext.Products.FindAsync(id) ?? null;
    }

    public async Task<Product> CreateProduct(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdateProduct(int id, Product updatedProduct)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
        {
            return null;
        }

        if (updatedProduct == null)
        {
            return null;
        }

        product!.UpdateProduct(updatedProduct.Name, updatedProduct.Description, updatedProduct.Price);

        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> DeleteProduct(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return null;
        }

        _dbContext.Products.Remove(product!);
        await _dbContext.SaveChangesAsync();

        return product;
    }
}

