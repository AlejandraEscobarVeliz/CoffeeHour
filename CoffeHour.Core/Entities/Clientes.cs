using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoffeHour.Core.Entities
{
    public partial class Clientes
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual ICollection<Pedidos>? Pedidos { get; set; }
    }
}
