using System.ComponentModel.DataAnnotations;

namespace VentasProject.Domain.Entities.Dwh.Facts
{
    public class FactVentas
    {
        [Key]
        public int VentaId { get; set; }
        public int Fk_Product { get; set; }
        public int Fk_Customer { get; set; }
        public int Fk_Date { get; set; }

        public int Quantity { get; set; }
        public decimal TotalVenta { get; set; }
        public string Status { get; set; } = null!;

    }
}
