using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VentasProject.Application.Repositories;
using VentasProject.Domain.Entities.Csv;
using VentasProject.Persistence.Repositories.Db.Context;

namespace VentasProject.Persistence.Repositories.Db
{
    public class SalesDbRepository : ISalesDbRepository
    {
        private readonly SalesContext _context;
        private readonly ILogger<SalesDbRepository> _logger;
        public SalesDbRepository(SalesContext context, ILogger<SalesDbRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            _logger.LogInformation("Iniciando Proceso");
            try
            {
                var query = from order in _context.Orders
                            join orderDetail in _context.OrderDetails
                            on order.OrderId equals orderDetail.OrderId
                            select new Sale
                            {
                                OrderId = order.OrderId,
                                CustomerId = order.CustomerId,
                                OrderDate = order.OrderDate,
                                Status = order.Status,
                                ProductId = orderDetail.ProductId,
                                Quantity = orderDetail.Quantity,
                                TotalPrice = orderDetail.TotalPrice
                            };
                var sale = await query.AsNoTracking().ToListAsync();
                _logger.LogInformation("Proceso Finalizado. Se han extraido {Count} registros", query.Count());
                return sale;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al obtener las ventas: {Message}", ex.Message);
                return Enumerable.Empty<Sale>();
            }
        }


    }
}
