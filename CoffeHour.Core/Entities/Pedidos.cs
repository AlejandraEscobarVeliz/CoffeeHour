using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Entities
{
    public partial class Pedidos
    {
        public int IdPedido { get; set; }
        public int? IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente";

        public virtual Clientes? Cliente { get; set; }
        public virtual ICollection<DetallesPedido>? DetallesPedido { get; set; }
    }
}
