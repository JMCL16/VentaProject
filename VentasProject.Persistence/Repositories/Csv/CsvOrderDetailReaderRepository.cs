using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VentasProject.Application.Repositories.Csv;
using VentasProject.Domain.Entities.Csv;
using CsvHelper;
using System.Globalization;

namespace VentasProject.Persistence.Repositories.Csv
{
    public class CsvOrderDetailReaderRepository : ICsvOrderDetailsReaderRepository
    {
        private readonly string? _filePath;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CsvOrderDetailReaderRepository> _logger;

        public CsvOrderDetailReaderRepository(IConfiguration configuration, ILogger<CsvOrderDetailReaderRepository> logger, string? filePath)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = filePath;
        }
        public async Task<IEnumerable<OrderDetails>> ReadFileAsync<T>()
        {
            List<OrderDetails> orderDetailsList = new();
            try
            {
                if (string.IsNullOrEmpty(_filePath))
                {
                    _logger.LogError("File path is not found {FilePath}", _filePath);
                    return orderDetailsList;
                }

                using var reader = new StreamReader(_filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                await foreach (var record in csv.GetRecordsAsync<OrderDetails>())
                {
                    orderDetailsList.Add(record);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading the file {FilePath}", _filePath);
            }
            return orderDetailsList;

        }
    }
}
