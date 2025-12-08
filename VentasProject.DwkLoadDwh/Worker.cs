using Microsoft.VisualBasic;
using System.Globalization;
using VentasProject.Application.Dtos;
using VentasProject.Application.Interfaces;
using VentasProject.Application.Repositories.Csv;
using VentasProject.Application.Repositories.Dwh;
using VentasProject.Application.Services;
using VentasProject.Domain.Entities.Csv;
using VentasProject.Domain.Entities.Dwh.Dimensions;
using VentasProject.Domain.Entities.Dwh.Facts;
// IMPORTANTE: Asegúrate de que este namespace tenga tus entidades DimCustomers, DimProducts y DimDate

namespace VentasProject.DwkLoadDwh
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker iniciado a las: {time}", DateTimeOffset.Now);

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // 1. Obtener Servicios
                    var customerRepository = scope.ServiceProvider.GetRequiredService<ICsvCustomerReaderRepository>();
                    var productRepository = scope.ServiceProvider.GetRequiredService<ICsvProductReaderRepository>();
                    var salesRepository = scope.ServiceProvider.GetRequiredService<ISalesRepository>();
                    var dwhHandlerService = scope.ServiceProvider.GetRequiredService<DwhHandlerService>();
                    var dwhRepository = scope.ServiceProvider.GetRequiredService<IDwhRepository>();

                    _logger.LogInformation("Limpiando datos previos en el DWH...");
                    await dwhRepository.CleanDataAsync();

                    _logger.LogInformation("--- INICIO DE EXTRACCIÓN (CSV) ---");

                    var customersTask = customerRepository.ReadFileAsync<Customers>();
                    var productsTask = productRepository.ReadFileAsync<Products>();
                    var salesTask = salesRepository.GetSalesDataAsync();

                    await Task.WhenAll(customersTask, productsTask, salesTask);

                    var csvCustomers = (await customersTask).ToList();
                    var csvProducts = (await productsTask).ToList();
                    var rawSalesData = (await salesTask).ToList();

                    _logger.LogInformation($"Datos Extraidos: Clientes: {csvCustomers.Count}, Productos: {csvProducts.Count}, Ventas: {rawSalesData.Count}");

                    _logger.LogInformation("--- INICIO DE TRANSFORMACIÓN ---");

                    await dwhHandlerService.TAndLDataToDwhAsync(csvCustomers, csvProducts, rawSalesData);

                    _logger.LogInformation("¡Carga de TODAS las Dimensiones Exitosa!");

                    _logger.LogInformation("Iniciando la Carga de facts");

                    await dwhHandlerService.LoadFactsToDwhAsync(rawSalesData);

                    _logger.LogInformation("¡PROCESO ETL COMPLETO FINALIZADO!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR FATAL en el Worker.");
            }
        }
    }
}