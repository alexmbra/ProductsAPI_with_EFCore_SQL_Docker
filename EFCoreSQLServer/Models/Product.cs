namespace EFCoreSQLServer.Models;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    public Product(int id, string name, string description, decimal price)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
    }

    // Method to update the product
    public void UpdateProduct(string newName, string newDescription, decimal newPrice)
    {
        Name = newName;
        Description = newDescription;
        Price = newPrice;
    }
}
