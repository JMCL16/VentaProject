using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasProject.Domain.Entities.Csv
{
    public class Sale
    {
        // Propiedades de Order
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string? Status { get; set; }

        // Propiedades de OrderDetail
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
