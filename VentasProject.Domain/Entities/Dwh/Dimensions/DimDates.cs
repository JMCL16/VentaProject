using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasProject.Domain.Entities.Dwh.Dimensions
{
    [Table("Dim_Date", Schema = "Dimension")]
    public class DimDates
    {
        [Key]
        public int DateKey { get; set; }
        public int DateId { get; set; }
        public DateTime Date { get; set; }
        public int Anio { get; set; }
        public int Trimestre { get; set; }
        public int Mes { get; set; }
        public string NombreMes { get; set; } = null!;
        public int Semana { get; set; }
        public int DiaMes { get; set; }
        public int DiaSemana { get; set; }
        public string NombreDia { get; set; } = null!;
    }
}
