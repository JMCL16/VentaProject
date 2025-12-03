using VentasProject.Application.Repositories.Base;
using VentasProject.Domain.Entities.Csv;


namespace VentasProject.Application.Repositories.Csv
{
    public interface ICsvOrderDetailsReaderRepository : IFileReaderRepository<OrderDetails>
    {
    }
}
