
using VentasProject.Application.Repositories.Csv;

namespace VentasProject.Persistence.Repositories.Csv
{
    using Domain.Entities.Csv;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System.IO;
    using CsvHelper;
    using System.Globalization;

    public class CsvCustomerReaderRepository : ICsvCustomerReaderRepository
    {
        private readonly string? _filePath;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CsvCustomerReaderRepository> _logger;

        public CsvCustomerReaderRepository(IConfiguration configuration, ILogger<CsvCustomerReaderRepository> logger, string? filePath)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = filePath;
        }

        public async Task<IEnumerable<Customers>> ReadFileAsync<T>()
        {
            List<Customers>? customersData = new();
            _logger.LogInformation("Reading CSV file at path: {Path}", _filePath);
            try
            {
                if (string.IsNullOrEmpty(_filePath))
                {
                    _logger.LogError("CSV file path is not found: {filePath}", _filePath);
                    return customersData;
                }

                using var reader = new StreamReader(_filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                await foreach (var record in csv.GetRecordsAsync<Customers>())
                {
                    customersData.Add(record);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading CSV file at path: {filePath}", _filePath);
                return Enumerable.Empty<Customers>();
            }
            return customersData!;
        }
    }
}
