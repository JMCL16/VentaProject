using Microsoft.EntityFrameworkCore;
using VentasProject.Api.Data.Entities;

namespace VentasProject.Api.Data.Context
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }

        public DbSet<Customer> Customer { get; set; }

    }
}
