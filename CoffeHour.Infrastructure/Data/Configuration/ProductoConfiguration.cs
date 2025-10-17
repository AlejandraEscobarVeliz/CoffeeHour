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
    public class ProductoConfiguration : IEntityTypeConfiguration<Productos>
    {
        public void Configure(EntityTypeBuilder<Productos> builder)
        {
            builder.HasKey(e => e.IdProducto).HasName("PK_Producto");

            builder.ToTable("Productos");

            builder.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(e => e.Categoria)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Precio)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(e => e.Estado)
                .HasMaxLength(10)
                .HasDefaultValue("Activo");

            builder.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
