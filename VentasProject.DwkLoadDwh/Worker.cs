using VentasProject.Application.Repositories.Csv;
using VentasProject.Domain.Entities.Csv;

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
                    var customerRepository = scope.ServiceProvider.GetRequiredService<ICsvCustomerReaderRepository>();
                    var productRepository = scope.ServiceProvider.GetRequiredService<ICsvProductReaderRepository>();
                    var salesRepository = scope.ServiceProvider.GetRequiredService<ISalesRepository>();

                    _logger.LogInformation("Extrayendo y trasformando Ventas...");
                    var salesTask = salesRepository.GetSalesDataAsync();

                    _logger.LogInformation("Extrayend customers del CSV...");
                    var customersTask = customerRepository.ReadFileAsync<Customers>();
                    _logger.LogInformation("Extrayendo products del CSV...");
                    var productsTask = productRepository.ReadFileAsync<Products>();

                    await Task.WhenAll(salesTask, customersTask, productsTask);

                    var salesData = (await salesTask).ToList();
                    var customersData = (await customersTask).ToList();
                    var productsData = (await productsTask).ToList();

                    _logger.LogInformation("=========== Resumen del Extract ========");
                    _logger.LogInformation("Total de Sales extraidos y transformados: {count}", salesData.Count());
                    _logger.LogInformation("Total de Customers extraidos: {count}", customersData.Count);
                    _logger.LogInformation("Total de Products extraidos: {count}", productsData.Count);
                    _logger.LogInformation("===================");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the CSV extraction process.");
            }
            finally
            {
                _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
            }
        }
    }
}
