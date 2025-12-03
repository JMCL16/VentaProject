using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VentasProject.Application.Repositories.Csv;
using VentasProject.Domain.Entities.Csv;
using CsvHelper;
using System.Globalization;

namespace VentasProject.Persistence.Repositories.Csv
{
    public class CsvOrderReaderRepository : ICsvOrderReaderRepository
    {
        private readonly string? _filePath;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CsvOrderReaderRepository> _logger;

        public CsvOrderReaderRepository(IConfiguration configuration, ILogger<CsvOrderReaderRepository> logger, string? filePath)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = filePath;
        }
        public async Task<IEnumerable<Orders>> ReadFileAsync<T>()
        {
            List<Orders> ordersList = new();

            try
            {
                if (string.IsNullOrEmpty(_filePath))
                {
                    _logger.LogError("File path is not found {FilePath}.", _filePath);
                }

                using var reader = new StreamReader(_filePath!);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                await foreach (var record in csv.GetRecordsAsync<Orders>())
                {
                    ordersList.Add(record);
                }
            }
            catch (Exception ex)
            {
                ordersList = null!;
                _logger.LogError(ex, "An error occurred while reading the CSV file at {FilePath}.", _filePath);

            }
            return ordersList;
        }
    }
}
