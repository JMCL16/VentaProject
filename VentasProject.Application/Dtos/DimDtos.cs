using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Domain.Entities.Dwh.Dimensions;

namespace VentasProject.Application.Dtos
{
    public class DimDtos
    {
        public List<DimCustomers> Customers { get; set; } = new List<DimCustomers>();
        public List<DimProducts> Products { get; set; } = new List<DimProducts>();
        public List<DatesDto>? Dates { get; set; }


    }
}
