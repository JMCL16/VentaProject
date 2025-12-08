using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Domain.Entities.Dwh.Dimensions;

namespace VentasProject.Persistence.Repositories.Dwh.Context
{
    public class DwhSalesContext : DbContext
    {
        public DwhSalesContext(DbContextOptions<DwhSalesContext> options) : base(options)
        {
        }

        public DbSet<DimCustomers> DimCustomers { get; set; }
        public DbSet<DimProducts> DimProducts { get; set; }
        public DbSet<DimDates> DimDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimCustomers>(entity =>
            {
                entity.ToTable("Dim_Customer", "Dimension");
                entity.HasKey(e => e.CustomerKey);

                entity.Property(e => e.CustomerKey).HasColumnName("CustomerKey");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerId");
                entity.Property(e => e.CustomerName).HasColumnName("CustomerName").HasMaxLength(200);
                entity.Property(e => e.City).HasColumnName("City").HasMaxLength(200);
                entity.Property(e => e.Country).HasColumnName("Country").HasMaxLength(200);
            });

            modelBuilder.Entity<DimProducts>(entity =>
            {
                entity.ToTable("Dim_Products", "Dimension");
                entity.HasKey(e => e.ProductKey);

                entity.Property(e => e.ProductKey).HasColumnName("ProductKey");
                entity.Property(e => e.ProductId).HasColumnName("ProductId");
                entity.Property(e => e.ProductName).HasColumnName("ProductName").HasMaxLength(100);
                entity.Property(e => e.Category).HasColumnName("Category").HasMaxLength(100);
                entity.Property(e => e.ListPrice).HasColumnName("ListPrice").HasColumnType("money");
            });

            modelBuilder.Entity<DimDates>(entity =>
            {
                entity.ToTable("Dim_Date", "Dimension");
                entity.HasKey(e => e.DateKey);

                entity.Property(e => e.DateKey).HasColumnName("DateKey");
                entity.Property(e => e.DateId).HasColumnName("DateId");
                entity.Property(e => e.Date).HasColumnName("Date").HasColumnType("date");
                entity.Property(e => e.Anio).HasColumnName("Anio");
                entity.Property(e => e.Trimestre).HasColumnName("Trimestre");
                entity.Property(e => e.Mes).HasColumnName("Mes");
                entity.Property(e => e.NombreMes).HasColumnName("NombreMes").HasMaxLength(50);
                entity.Property(e => e.Semana).HasColumnName("Semana");
                entity.Property(e => e.DiaMes).HasColumnName("DiaMes");
                entity.Property(e => e.DiaSemana).HasColumnName("DiaSemana");
                entity.Property(e => e.NombreDia).HasColumnName("NombreDia").HasMaxLength(50);
            });
        }
    }
}
