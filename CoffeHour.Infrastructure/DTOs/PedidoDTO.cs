using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Infrastructure.DTOs
{
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public int? IdCliente { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente";

        public List<DetallePedidoDTO> DetallesPedido { get; set; } = new();
        
    }
}
