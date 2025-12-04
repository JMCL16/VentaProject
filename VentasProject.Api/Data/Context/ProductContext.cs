using Microsoft.EntityFrameworkCore;
using VentasProject.Api.Data.Entities;

namespace VentasProject.Api.Data.Context
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }

    }
}

