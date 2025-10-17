using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace CoffeHour.Core.DTOs
{
    public class SalesReportDTO
    {
        public DateTime Fecha { get; set; }
        public int OrdersCount { get; set; }
        public decimal TotalSales { get; set; }
    }
}

 