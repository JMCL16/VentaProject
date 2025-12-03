
using Microsoft.Extensions.Logging;
using VentasProject.Application.Repositories.Csv;
using VentasProject.Domain.Entities.Csv;

namespace VentasProject.Persistence.Repositories.Csv
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ICsvOrderDetailsReaderRepository _csvOrderDetailsReaderRepository;
        private readonly ICsvOrderReaderRepository _csvOrderReaderRepository;
        private readonly ILogger<SalesRepository> _logger;

        public SalesRepository(
            ICsvOrderDetailsReaderRepository csvOrderDetailsReaderRepository,
            ICsvOrderReaderRepository csvOrderReaderRepository,
            ILogger<SalesRepository> logger)
        {
            _csvOrderDetailsReaderRepository = csvOrderDetailsReaderRepository;
            _csvOrderReaderRepository = csvOrderReaderRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<Sale>> GetSalesDataAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando la extraccion de order");
                var orderTask = _csvOrderReaderRepository.ReadFileAsync<Orders>();

                _logger.LogInformation("Iniciando la extraccion de order details");
                var orderDetailsTask = _csvOrderDetailsReaderRepository.ReadFileAsync<OrderDetails>();
                Task.WhenAll(orderTask, orderDetailsTask);
                var orders = (await orderTask).ToList();
                var orderDetails = (await orderDetailsTask).ToList();
                _logger.LogInformation("Completada la extraccion de datos. Datos de orders {Count}, Order Details {Count}", orders.Count, orderDetails.Count);

                _logger.LogInformation("Iniciando la transformacion de datos de ventas");
                var salesData = from order in orders
                                join detail in orderDetails
                                on order.OrderId equals detail.OrderId
                                select new Sale
                                {
                                    //Order informacion
                                    OrderId = order.OrderId,
                                    CustomerId = order.CustomerId,
                                    OrderDate = order.OrderDate,
                                    Status = order.Status,

                                    //Order details informacion
                                    ProductId = detail.ProductId,
                                    Quantity = detail.Quantity,
                                    TotalPrice = detail.TotalPrice
                                };
                var salesDataList = salesData.ToList();
                _logger.LogInformation("Transformacion de datos completada. Total de registros de ventas: {Count}", salesData.Count());
                return salesDataList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los datos de ventas desde los archivos CSV");
                return Enumerable.Empty<Sale>();
            }
        }
    }
}
