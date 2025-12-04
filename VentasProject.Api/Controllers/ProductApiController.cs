using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VentasProject.Api.Data.Interface;

namespace VentasProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductApiRepository _repo;

        public ProductApiController(IProductApiRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProductsAsync()
        {
            var products = await _repo.GetAllProductAsync();
            return Ok(products);
        }

    }
}
