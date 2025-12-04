using Microsoft.EntityFrameworkCore;
using VentasProject.Api.Data.Context;
using VentasProject.Api.Data.Entities;
using VentasProject.Api.Data.Interface;

namespace VentasProject.Api.Data.Repositories
{
    public class CustomerApiRepository : ICustomerApiRepository
    {
        private readonly CustomerContext _customerContext;

        public CustomerApiRepository(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerContext.Customer.ToListAsync();
        }
    }
}
