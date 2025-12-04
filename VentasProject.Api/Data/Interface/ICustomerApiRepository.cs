using VentasProject.Api.Data.Entities;

namespace VentasProject.Api.Data.Interface
{
    public interface ICustomerApiRepository
    {
        Task<List<Customer>> GetAllCustomersAsync();
    }
}
