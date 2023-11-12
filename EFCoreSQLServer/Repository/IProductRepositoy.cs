using EFCoreSQLServer.Models;

namespace EFCoreSQLServer.Repository;
public interface IProductRepositoy
{
    Task<Product> CreateProduct(Product product);
    Task<Product?> DeleteProduct(int id);
    Task<Product?> GetProductById(int id);
    Task<IEnumerable<Product>> GetProducts();
    Task<Product?> UpdateProduct(int id, Product updatedProduct);
}