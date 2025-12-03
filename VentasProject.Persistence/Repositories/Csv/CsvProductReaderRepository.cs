using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VentasProject.Application.Repositories.Csv;
using VentasProject.Domain.Entities.Csv;
using CsvHelper;
using System.Globalization;

namespace VentasProject.Persistence.Repositories.Csv
{
    public class CsvProductReaderRepository : ICsvProductReaderRepository
    {
        private readonly string? _filePath;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CsvProductReaderRepository> _logger;

        public CsvProductReaderRepository(IConfiguration configuration, ILogger<CsvProductReaderRepository> logger, string? filePath)
        {
            _configuration = configuration;
            _logger = logger;
            _filePath = filePath;
        }

        public async Task<IEnumerable<Products>> ReadFileAsync<T>()
        {
            List<Products> products = new();
            try
            {
                if (string.IsNullOrEmpty(_filePath))
                {
                    _logger.LogError("File path is not found {FilePath}", _filePath);
                    return products;
                }

                using var streamReader = new StreamReader(_filePath);
                using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

                await foreach (var record in csvReader.GetRecordsAsync<Products>())
                {
                    products.Add(record);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading the CSV file at path: {FilePath}", _filePath);
            }
            return products;
        }
    }


}
