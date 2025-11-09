using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Entities
{
    public partial class Pedidos: BaseEntity
    {
        [Key]
        [Column("IdPedido")] // Mapea "Id" a la columna real "IdPedido"
        public new int Id { get; set; }
        [Required]
        [Column("IdCliente")]
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente";

        public virtual Clientes? Cliente { get; set; }
        public virtual ICollection<DetallesPedido>? DetallesPedido { get; set; }
    }
}
