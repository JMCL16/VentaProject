using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Application.Services;
using VentasProject.Domain.Entities.Csv;

namespace VentasProject.Application.Repositories.Dwh
{
    public interface IDwhHandlerService
    {
        Task<Result> TAndLDataToDwhAsync(List<Customers> customers, List<Products> products, List<Sale> orders);
    }
}
