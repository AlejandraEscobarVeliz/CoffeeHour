using CoffeHour.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Infrastructure.Data.Configuration
{
    public class DetallePedidoConfiguration : IEntityTypeConfiguration<DetallesPedido>
    {
        public void Configure(EntityTypeBuilder<DetallesPedido> builder)
        {
            builder.HasKey(e => e.IdDetalle).HasName("PK_DetallePedido");

            builder.ToTable("Detalles_Pedido");

            builder.Property(e => e.Cantidad)
                .IsRequired();

            builder.Property(e => e.Subtotal)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            // 🔹 Relación: cada detalle pertenece a un pedido
            builder.HasOne(e => e.Pedido)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(e => e.IdPedido)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Detalle_Pedido");

            // 🔹 Relación: cada detalle pertenece a un producto
            builder.HasOne(e => e.Producto)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(e => e.IdProducto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Detalle_Producto");
        }
    }
}
