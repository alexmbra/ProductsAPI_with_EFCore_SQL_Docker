using AutoBogus;
using EFCoreSQLServer.Context;
using EFCoreSQLServer.Controllers;
using EFCoreSQLServer.Models;
using EFCoreSQLServer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSQLServer.Tests;

[TestFixture]
internal class ProductsControllerTests
{
    private ProductsController _controller;
    private DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private ApplicationDbContext _dbContext;
    private ProductRepositoy _productRepository;
    private List<Product> _sampleProducts;
    private readonly int numberOfProducts = 5;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;

        _dbContext = new ApplicationDbContext(_dbContextOptions);

        _sampleProducts = AutoFaker.Generate<Product>(numberOfProducts);

        if (_dbContext.Database.IsInMemory())
        {
            _dbContext.Products.AddRange(_sampleProducts);
            _dbContext.SaveChanges();
        }
        _productRepository = new ProductRepositoy(_dbContext);
        _controller = new ProductsController(_productRepository);
    }

    [TearDown]
    public void TearDown()
    {
        using var dbContext = new ApplicationDbContext(_dbContextOptions);
        if (dbContext.Database.IsInMemory())
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        _dbContext.Dispose();
    }

    [Test]
    public async Task GetProducts_ReturnsOkResult()
    {
        // Act
        var result = await _controller.GetProduct();

        // Assert
        Assert.That(result.Result, Is.Not.Null, "Result should not be null");
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result.Result;
        Assert.That(okResult.Value, Is.Not.Null, "okResult should not be null");
        var returnedProducts = (IEnumerable<Product>)okResult.Value;
        Assert.That(_sampleProducts, Has.Count.EqualTo(returnedProducts.Count()));
    }



    [Test]
    public async Task CreateProduct_ReturnsOkResult()
    {
        // Arrange
        var product = AutoFaker.Generate<Product>();

        // Act
        var result = await _controller.Create(product);

        // Assert
        Assert.That(result.Result, Is.Not.Null, "Result should not be null");
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result.Result;
        Assert.That(okResult.Value, Is.Not.Null, "okResult should not be null");
        var createdProduct = (Product)okResult.Value;
        Assert.That(product, Is.EqualTo(createdProduct));
    }

    [Test]
    public async Task UpdateProduct_ReturnsOkResult()
    {
        // Arrange
        var existingProduct = _sampleProducts.First();
        var updatedProduct = AutoFaker.Generate<Product>();

        // Act
        var result = await _controller.Update(existingProduct.Id, updatedProduct);

        // Assert
        Assert.That(result.Result, Is.Not.Null, "Result should not be null");
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result.Result;
        Assert.That(okResult.Value, Is.Not.Null, "okResult should not be null");
        var returnedProduct = (Product)okResult.Value;
        Assert.Multiple(() =>
        {
            Assert.That(updatedProduct.Name, Is.EqualTo(returnedProduct.Name));
            Assert.That(updatedProduct.Description, Is.EqualTo(returnedProduct.Description));
            Assert.That(updatedProduct.Price, Is.EqualTo(returnedProduct.Price));
        });
    }

    [Test]
    public async Task DeleteProduct_ReturnsOkResult()
    {
        // Arrange
        var productToDelete = _sampleProducts.First();

        // Act
        var result = await _controller.Delete(productToDelete.Id);

        // Assert
        Assert.That(result.Result, Is.Not.Null, "Result should not be null");
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result.Result;
        Assert.That(okResult.Value, Is.Not.Null, "okResult should not be null");
        var deletedProduct = (Product)okResult.Value;
        Assert.That(productToDelete, Is.EqualTo(deletedProduct));
    }
}
