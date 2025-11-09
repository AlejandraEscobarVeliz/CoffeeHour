using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using CoffeHour.Core.Entities;


namespace CoffeHour.Infrastructure.Data
{

    public partial class CoffeeHourContext : DbContext
    {
        public CoffeeHourContext()
        {

        }

        public CoffeeHourContext(DbContextOptions<CoffeeHourContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Pedidos> Pedidos { get; set; }
        public virtual DbSet<DetallesPedido> DetallesPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

