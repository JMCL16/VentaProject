using Microsoft.VisualBasic;
using System.Globalization;
using VentasProject.Application.Dtos;
using VentasProject.Application.Interfaces;
using VentasProject.Application.Repositories.Csv;
using VentasProject.Application.Repositories.Dwh;
using VentasProject.Domain.Entities.Csv;
using VentasProject.Domain.Entities.Dwh.Dimensions;
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
                    var dwhRepository = scope.ServiceProvider.GetRequiredService<IDwhRepository>();

                    _logger.LogInformation("--- INICIO DE EXTRACCIÓN (CSV) ---");

                    var customersTask = customerRepository.ReadFileAsync<Customers>();
                    var productsTask = productRepository.ReadFileAsync<Products>();

                    await Task.WhenAll(customersTask, productsTask);

                    var csvCustomers = (await customersTask).ToList();
                    var csvProducts = (await productsTask).ToList();

                    _logger.LogInformation($"Datos CSV -> Clientes: {csvCustomers.Count}, Productos: {csvProducts.Count}");

                    _logger.LogInformation("--- INICIO DE TRANSFORMACIÓN ---");

                    var dimData = new DimDtos();

                    // A. Transformar Clientes (CORREGIDO NOMBRE COMPLETO)
                    dimData.Customers = csvCustomers.Select(c => new DimCustomers
                    {
                        CustomerId = c.CustomerId,
                        // AQUÍ ESTABA EL ERROR DEL NOMBRE:
                        CustomerName = $"{c.FirstName} {c.LastName}".Trim(),
                        City = c.City ?? "Desconocido",
                        Country = c.Country ?? "Desconocido"
                    }).ToList();

                    // B. Transformar Productos
                    dimData.Products = csvProducts.Select(p => new DimProducts
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        Category = p.Category ?? "Sin Categoría",
                        ListPrice = p.Price
                    }).ToList();

                    // C. Generar Fechas (ESTO FALTABA)
                    _logger.LogInformation("Generando Dimensión de Fechas...");
                    dimData.Dates = GenerateDates(2020, 2030);

                    // 4. CARGA
                    _logger.LogInformation($"--- CARGANDO {dimData.Dates.Count} FECHAS, {dimData.Customers.Count} CLIENTES Y {dimData.Products.Count} PRODUCTOS ---");

                    await dwhRepository.LoadDimsDataAsync(dimData);

                    _logger.LogInformation("¡Carga de TODAS las Dimensiones Exitosa!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR FATAL en el Worker.");
            }
        }

        // Método auxiliar para generar fechas
        private List<DimDates> GenerateDates(int startYear, int endYear)
        {
            var datesList = new List<DimDates>();
            var startDate = new DateTime(startYear, 1, 1);
            var endDate = new DateTime(endYear, 12, 31);
            var culture = new CultureInfo("es-ES");

            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                datesList.Add(new DimDates
                {
                    DateId = (dt.Year * 10000) + (dt.Month * 100) + dt.Day,
                    Date = dt,
                    Anio = dt.Year,
                    Trimestre = ((dt.Month - 1) / 3) + 1,
                    Mes = dt.Month,
                    NombreMes = culture.TextInfo.ToTitleCase(dt.ToString("MMMM", culture)),
                    Semana = culture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday),
                    DiaMes = dt.Day,
                    DiaSemana = (int)dt.DayOfWeek,
                    NombreDia = culture.TextInfo.ToTitleCase(dt.ToString("dddd", culture))
                });
            }
            return datesList;
        }
    }
}