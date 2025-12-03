namespace VentasProject.Application.Repositories.Csv
{
    using VentasProject.Application.Repositories.Base;
    using VentasProject.Domain.Entities.Csv;
    public interface ICsvProductReaderRepository : IFileReaderRepository<Products>
    {
    }
}
