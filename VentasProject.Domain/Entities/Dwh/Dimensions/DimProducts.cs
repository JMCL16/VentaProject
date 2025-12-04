using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasProject.Domain.Entities.Dwh.Dimensions
{
    [Table("Dim_Products", Schema = "Dimension")]
    public class DimProducts
    {
        [Key]
        public int ProductKey { get; set; } 
        public int ProductId { get; set; } 
        public string? ProductName { get; set; }
        public string? Category { get; set; }
        public decimal? ListPrice { get; set; }
    }
}
