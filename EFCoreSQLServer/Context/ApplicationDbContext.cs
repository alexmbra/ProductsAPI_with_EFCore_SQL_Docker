using EFCoreSQLServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSQLServer.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    public DbSet<Product> Products { get; set; }
}
