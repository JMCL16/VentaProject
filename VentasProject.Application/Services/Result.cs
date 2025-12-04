using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasProject.Application.Services
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; } = string.Empty;
    }
}
