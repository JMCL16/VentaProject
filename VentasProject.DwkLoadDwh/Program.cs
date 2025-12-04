using Microsoft.EntityFrameworkCore;
using VentasProject.Application.Interfaces;
using VentasProject.Application.Repositories.Csv;
using VentasProject.Application.Repositories.Dwh;
using VentasProject.Application.Services;
using VentasProject.DwkLoadDwh;
using VentasProject.Persistence.Repositories.Csv;
using VentasProject.Persistence.Repositories.Dwh;
using VentasProject.Persistence.Repositories.Dwh.Context;

namespace VentasProject.DwkLoadDwh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            var configuration = builder.Configuration;

            string customersFilePath = configuration.GetValue<string>("Csv:CustomerFilePath")!;
            string productsFilePath = configuration.GetValue<string>("Csv:ProductFilePath")!;
            string ordersFilePath = configuration.GetValue<string>("Csv:OrderFilePath")!;
            string orderDetailsFilePath = configuration.GetValue<string>("Csv:OrderDetailFilePath")!;

            builder.Services.AddTransient<ICsvCustomerReaderRepository>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<CsvCustomerReaderRepository>>();
                return new CsvCustomerReaderRepository(configuration, logger, customersFilePath);
            });

            builder.Services.AddTransient<ICsvProductReaderRepository>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<CsvProductReaderRepository>>();
                return new CsvProductReaderRepository(configuration, logger, productsFilePath);
            });

            builder.Services.AddTransient<ICsvOrderDetailsReaderRepository>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<CsvOrderDetailReaderRepository>>();
                return new CsvOrderDetailReaderRepository(configuration, logger, orderDetailsFilePath);
            });

            builder.Services.AddTransient<ICsvOrderReaderRepository>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<CsvOrderReaderRepository>>();
                return new CsvOrderReaderRepository(configuration, logger, ordersFilePath);
            });

            builder.Services.AddDbContext<DwhSalesContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DwhConnection")));

            builder.Services.AddScoped<IDwhRepository, DwhRepository>();

            builder.Services.AddScoped<DwhHandlerService>();

            builder.Services.AddScoped<ISalesRepository, SalesRepository>();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();

        }
    }
}



