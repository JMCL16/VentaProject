using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VentasProject.Api.Data.Interface;

namespace VentasProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly ICustomerApiRepository _repo;

        public CustomerApiController(ICustomerApiRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var customers = await _repo.GetAllCustomersAsync();
            return Ok(customers);
        }
    }
}
