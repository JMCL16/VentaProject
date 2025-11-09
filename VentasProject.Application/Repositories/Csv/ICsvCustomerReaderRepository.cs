using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Application.Repositories.Base;
using VentasProject.Domain.Entities.Csv;


namespace VentasProject.Application.Repositories.Csv
{
    public interface ICsvCustomerReaderRepository : IFileReaderRepository<Customers>
    {
    }
}
