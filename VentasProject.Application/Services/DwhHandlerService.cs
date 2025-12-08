using Microsoft.Extensions.Logging;

using System.Globalization;
using VentasProject.Application.Dtos;
using VentasProject.Application.Repositories.Dwh;
using VentasProject.Domain.Entities.Csv;
using VentasProject.Domain.Entities.Dwh.Dimensions;

namespace VentasProject.Application.Services
{
    public class DwhHandlerService : IDwhHandlerService
    {
        private readonly IDwhRepository _dwhRepository;
        private readonly ILogger<DwhHandlerService> _logger;   
        
        public DwhHandlerService(IDwhRepository dwhRepository, ILogger<DwhHandlerService> logger)
        {
            _dwhRepository = dwhRepository;
            _logger = logger;
        }
        public async Task<Result> TAndLDataToDwhAsync(List<Customers> rawCustomers, List<Products> rawProducts, List<Sale> sales)
        {
            _logger.LogInformation("Iniciando Transformación de datos...");

            var dimDtos = new DimDtos();

            dimDtos.Customers = rawCustomers.Select(c => new DimCustomers
            {
                CustomerId = c.CustomerId,
                CustomerName = $"{c.FirstName} {c.LastName ?? ""}".Trim(),
                City = c.City ?? "Desconocido",
                Country = c.Country ?? "Desconocido"
            }).ToList();

            dimDtos.Products = rawProducts.Select(p => new DimProducts
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName ?? "Desconocido",
                Category = p.Category ?? "Sin Categoría",
                ListPrice = p.Price
            }).ToList();

            _logger.LogInformation("Calculando Dimensión Tiempo...");

            var uniqueDates = sales
                .Select(s => s.OrderDate)
                .Distinct()
                .ToList();

            var culture = new CultureInfo("es-ES");

            dimDtos.Dates = uniqueDates.Select(d =>
            {
                DateTime dt = d.ToDateTime(TimeOnly.MinValue);

                return new DimDates
                {
                    DateId = (dt.Year * 10000) + (dt.Month * 100) + dt.Day,
                    Date = dt,
                    Anio = dt.Year,
                    Mes = dt.Month,
                    DiaMes = dt.Day,
                    DiaSemana = (int)dt.DayOfWeek,
                    Trimestre = ((dt.Month - 1) / 3) + 1,
                    Semana = culture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday),
                    NombreMes = culture.TextInfo.ToTitleCase(dt.ToString("MMMM", culture)),
                    NombreDia = culture.TextInfo.ToTitleCase(dt.ToString("dddd", culture))
                };
            }).ToList();

            _logger.LogInformation("Datos transformados. Enviando al Repositorio...");

            return await _dwhRepository.LoadDimsDataAsync(dimDtos);
        }
    }
}
