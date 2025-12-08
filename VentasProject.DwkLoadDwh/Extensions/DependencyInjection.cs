using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Application.Repositories.Csv;
using VentasProject.Application.Repositories.Dwh;
using VentasProject.Application.Services;
using VentasProject.Persistence.Repositories.Csv;
using VentasProject.Persistence.Repositories.Db;
using VentasProject.Persistence.Repositories.Db.Context;
using VentasProject.Persistence.Repositories.Dwh;
using VentasProject.Persistence.Repositories.Dwh.Context;

namespace VentasProject.DwkLoadDwh.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Configuración de Base de Datos DESTINO (Data Warehouse)
            services.AddDbContext<DwhSalesContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DwhConnection")));

            // 2. Configuración de Base de Datos ORIGEN (AnalisisVentas)
            services.AddDbContext<SalesContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OlapConnection")));

            // 3. Repositorios de Base de Datos
            services.AddScoped<IDwhRepository, DwhRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>(); 

            return services;
        }

        public static IServiceCollection AddCsvRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            string customersPath = configuration["Csv:CustomerFilePath"]!;
            string productsPath = configuration["Csv:ProductFilePath"]!;
            string ordersPath = configuration["Csv:OrderFilePath"]!;
            string orderDetailsPath = configuration["Csv:OrderDetailFilePath"]!;

            services.AddTransient<ICsvCustomerReaderRepository>(sp =>
                new CsvCustomerReaderRepository(configuration, sp.GetRequiredService<ILogger<CsvCustomerReaderRepository>>(), customersPath));

            services.AddTransient<ICsvProductReaderRepository>(sp =>
                new CsvProductReaderRepository(configuration, sp.GetRequiredService<ILogger<CsvProductReaderRepository>>(), productsPath));

            services.AddTransient<ICsvOrderReaderRepository>(sp =>
                new CsvOrderReaderRepository(configuration, sp.GetRequiredService<ILogger<CsvOrderReaderRepository>>(), ordersPath));

            services.AddTransient<ICsvOrderDetailsReaderRepository>(sp =>
                new CsvOrderDetailReaderRepository(configuration, sp.GetRequiredService<ILogger<CsvOrderDetailReaderRepository>>(), orderDetailsPath));

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Servicios de Negocio y Workers
            services.AddScoped<DwhHandlerService>();
            services.AddHostedService<Worker>();

            return services;
        }
    }
}
