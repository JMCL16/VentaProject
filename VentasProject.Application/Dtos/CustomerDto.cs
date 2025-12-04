using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasProject.Application.Dtos
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
