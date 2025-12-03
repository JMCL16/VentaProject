using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasProject.Application.Repositories.Base
{
    public interface IFileReaderRepository<TClass> where TClass : class
    {
        Task<IEnumerable<TClass>> ReadFileAsync<T>();
    }
}
