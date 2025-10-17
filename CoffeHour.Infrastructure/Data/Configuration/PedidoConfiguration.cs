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
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedidos>
    {
        public void Configure(EntityTypeBuilder<Pedidos> builder)
        {
            builder.HasKey(e => e.IdPedido).HasName("PK_Pedido");

            builder.ToTable("Pedidos");

            builder.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.Total)
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.Estado)
                .HasMaxLength(15)
                .HasDefaultValue("Pendiente");

            // 🔹 Relación: Pedido pertenece a un Cliente
            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Pedido_Cliente");
        }
    }
}
