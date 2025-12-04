using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasProject.Application.Dtos
{
    public class DatesDto
    {
        public int DateId { get; set; }
        public DateTime FullDate { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int Week { get; set; }
        public int DayOfMonth { get; set; }
        public int DayOfWeek { get; set; }
        public string DayName { get; set; } = string.Empty;
    }
}
