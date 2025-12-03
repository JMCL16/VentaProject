using VentasProject.Domain.Entities.Csv;

namespace VentasProject.Application.Repositories.Csv
{
    public interface ISalesRepository
    {
        public Task<IEnumerable<Sale>> GetSalesDataAsync();
    }
}
