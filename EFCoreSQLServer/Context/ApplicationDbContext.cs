using AutoBogus;
using EFCoreSQLServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSQLServer.Context;

public class ApplicationDbContext : DbContext
{
    private readonly int numberOfProducts = 10;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        if (!modelBuilder.Entity<Product>().Metadata.GetProperties().Any(p => p.Name == "ProductId"))
        {
            var _sampleProducts = AutoFaker.Generate<Product>(numberOfProducts);

            modelBuilder.Entity<Product>().HasData(_sampleProducts);
        }
    }
}
