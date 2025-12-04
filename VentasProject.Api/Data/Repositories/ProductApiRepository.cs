using Microsoft.EntityFrameworkCore;
using VentasProject.Api.Data.Context;
using VentasProject.Api.Data.Entities;
using VentasProject.Api.Data.Interface;

namespace VentasProject.Api.Data.Repositories
{
    public class ProductApiRepository : IProductApiRepository
    {
        private readonly ProductContext _context; 

        public ProductApiRepository(ProductContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllProductAsync()
        {
            return await _context.Product.ToListAsync();
        }
    }
}
