using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Application.Dtos;
using VentasProject.Application.Services;
using VentasProject.Domain.Entities.Dwh.Facts;

namespace VentasProject.Application.Repositories.Dwh
{
    public interface IDwhRepository
    {
        Task<Result> LoadDimsDataAsync(DimDtos dimDtos);
        Task CleanDataAsync();

        Task<Dictionary<int, int>> GetCustomerKeysMapAsync();
        Task<Dictionary<int, int>> GetProductKeysMapAsync();
        Task<Dictionary<int, int>> GetDateKeysMapAsync();
        Task<Result> LoadFactsAsync(List<FactVentas> facts);

    }
}
