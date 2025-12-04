using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Application.Dtos;

namespace VentasProject.Application.Interfaces
{
    public interface IDbExtractor
    {
        Task<List<CustomerDto>> ExtractCustomersFromDbAsync();
    }
}
