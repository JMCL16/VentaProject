using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Domain.Entities.Csv;

namespace VentasProject.Application.Repositories
{
    public interface ISalesDbRepository
    {
        Task<IEnumerable<Sale>> GetAllSalesAsync();
    }
}
