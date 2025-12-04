//using VentasProject.Application.Repositories.Csv;
//using VentasProject.Domain.Entities.Csv;

//namespace VentasProject.DwkLoadDwh
//{
//    public class Worker : BackgroundService
//    {
//        private readonly ILogger<Worker> _logger;
//        private readonly IServiceScopeFactory _serviceScopeFactory;

//        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
//        {
//            _logger = logger;
//            _serviceScopeFactory = serviceScopeFactory;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

//            try
//            {
//                using (var scope = _serviceScopeFactory.CreateScope())
//                {
//                    var customerRepository = scope.ServiceProvider.GetRequiredService<ICsvCustomerReaderRepository>();
//                    var productRepository = scope.ServiceProvider.GetRequiredService<ICsvProductReaderRepository>();
//                    var salesRepository = scope.ServiceProvider.GetRequiredService<ISalesRepository>();

//                    _logger.LogInformation("Extrayendo y trasformando Ventas...");
//                    var salesTask = salesRepository.GetSalesDataAsync();

//                    _logger.LogInformation("Extrayend customers del CSV...");
//                    var customersTask = customerRepository.ReadFileAsync<Customers>();
//                    _logger.LogInformation("Extrayendo products del CSV...");
//                    var productsTask = productRepository.ReadFileAsync<Products>();

//                    await Task.WhenAll(salesTask, customersTask, productsTask);

//                    var salesData = (await salesTask).ToList();
//                    var customersData = (await customersTask).ToList();
//                    var productsData = (await productsTask).ToList();

//                    _logger.LogInformation("=========== Resumen del Extract ========");
//                    _logger.LogInformation("Total de Sales extraidos y transformados: {count}", salesData.Count());
//                    _logger.LogInformation("Total de Customers extraidos: {count}", customersData.Count);
//                    _logger.LogInformation("Total de Products extraidos: {count}", productsData.Count);
//                    _logger.LogInformation("===================");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred during the CSV extraction process.");
//            }
//            finally
//            {
//                _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
//            }
//        }
//    }
//}

using VentasProject.Application.Dtos;       // Necesario para DimDtos
using VentasProject.Application.Interfaces; // Necesario para IDwhRepository
using VentasProject.Application.Repositories.Csv;
using VentasProject.Application.Repositories.Dwh;
using VentasProject.Domain.Entities;        // Necesario para DimCustomer/DimProduct (Entidades DWH)
using VentasProject.Domain.Entities.Csv;
using VentasProject.Domain.Entities.Dwh.Dimensions;

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
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // 1. Obtener los repositorios CSV (Tu código existente)
                    var customerRepository = scope.ServiceProvider.GetRequiredService<ICsvCustomerReaderRepository>();
                    var productRepository = scope.ServiceProvider.GetRequiredService<ICsvProductReaderRepository>();

                    // 2. NUEVO: Obtener el repositorio del DWH
                    var dwhRepository = scope.ServiceProvider.GetRequiredService<IDwhRepository>();

                    _logger.LogInformation("--- INICIO DE EXTRACCIÓN (CSV) ---");

                    // Extracción (Tu código existente)
                    var customersTask = customerRepository.ReadFileAsync<Customers>(); // CSV Entities
                    var productsTask = productRepository.ReadFileAsync<Products>();   // CSV Entities

                    await Task.WhenAll(customersTask, productsTask);

                    var csvCustomers = (await customersTask).ToList();
                    var csvProducts = (await productsTask).ToList();

                    _logger.LogInformation($"Datos extraídos CSV -> Clientes: {csvCustomers.Count}, Productos: {csvProducts.Count}");

                    // 3. NUEVO: TRANSFORMACIÓN (Mapeo CSV -> DWH Entity)
                    _logger.LogInformation("--- INICIO DE TRANSFORMACIÓN ---");

                    var dimData = new DimDtos();

                    // Transformar Clientes
                    dimData.Customers = csvCustomers.Select(c => new DimCustomers
                    {
                        // Asegúrate que las propiedades coincidan con tu Entidad de Dominio
                        CustomerId = c.CustomerId,      // Asumiendo que tu CSV tiene Id
                        CustomerName = c.FirstName,
                        City = c.City ?? "Desconocido",
                        Country = c.Country ?? "Desconocido"
                    }).ToList();

                    // Transformar Productos
                    dimData.Products = csvProducts.Select(p => new DimProducts
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        Category = p.Category ?? "Sin Categoría",
                        ListPrice = p.Price
                    }).ToList();

                    // 4. NUEVO: CARGA (Load)
                    _logger.LogInformation("--- CARGANDO EN DATA WAREHOUSE ---");

                    await dwhRepository.LoadDimsDataAsync(dimData);

                    _logger.LogInformation("¡Carga de Dimensiones Exitosa!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error durante el proceso de Carga de Dimensiones.");
            }
            finally
            {
                _logger.LogInformation("Worker finished cycle at: {time}", DateTimeOffset.Now);
            }
        }
    }
}