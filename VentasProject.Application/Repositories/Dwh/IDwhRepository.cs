using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Application.Dtos;
using VentasProject.Application.Services;

namespace VentasProject.Application.Repositories.Dwh
{
    public interface IDwhRepository
    {
        Task<Result> LoadDimsDataAsync(DimDtos dimDtos);
        Task CleanDataAsync();
    }
}
