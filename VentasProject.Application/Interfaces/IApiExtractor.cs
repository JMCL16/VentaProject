using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Application.Dtos;

namespace VentasProject.Application.Interfaces
{
    public interface IApiExtractor
    {
        Task<List<ProductDto>> ExtractProductosFromApiAsync();
        Task<List<CustomerDto>> ExtractCustomerFromApiAsync();
    }
}
