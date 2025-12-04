using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasProject.Domain.Entities.Dwh.Dimensions
{
    [Table("Dim_Customer", Schema ="Dimension")]
    public class DimCustomers
    {
        [Key]
        public int CustomerKey { get; set; } 
        public int CustomerId { get; set; }  
        public string? CustomerName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
