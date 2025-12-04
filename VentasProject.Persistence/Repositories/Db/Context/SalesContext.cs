using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Domain.Entities.Csv;
using VentasProject.Domain.Entities.Dwh.Dimensions;

namespace VentasProject.Persistence.Repositories.Db.Context
{
    public class SalesContext : DbContext
    {
        public SalesContext(DbContextOptions<SalesContext> options) : base(options)
        {
        }

        public DbSet<Orders> Orders { get; set; }   
        public DbSet<OrderDetails> OrderDetails { get; set; }

    }
}
