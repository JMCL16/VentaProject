using VentasProject.Api.Data.Entities;
namespace VentasProject.Api.Data.Interface
{
    public interface IProductApiRepository
    {
        Task<List<Product>> GetAllProductAsync(); 
    }
}
