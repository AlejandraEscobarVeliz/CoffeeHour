using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace CoffeHour.Infrastructure.Repositories
{
    public class DetallePedidoRepository : BaseRepository<DetallesPedido>, IDetallePedidoRepository
    {

        public DetallePedidoRepository(CoffeeHourContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DetallesPedido>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _entities
                .Where(d => d.IdPedido == pedidoId)
                .Include(d => d.Producto)
                .ToListAsync();
        }

        /*
        public async Task<IEnumerable<DetallesPedido>> GetAllAsync() =>
            await _context.DetallesPedido
                .Include(d => d.Producto)
                .Include(d => d.Pedido)
                .ToListAsync();

        public async Task<DetallesPedido?> GetByIdAsync(int id) =>
            await _context.DetallesPedido
                .Include(d => d.Producto)
                .Include(d => d.Pedido)
                .FirstOrDefaultAsync(d => d.Id == id);

        public async Task AddAsync(DetallesPedido detalle)
        {
            _context.DetallesPedido.Add(detalle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DetallesPedido detalle)
        {
            _context.DetallesPedido.Update(detalle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var detalle = await _context.DetallesPedido.FindAsync(id);
            if (detalle != null)
            {
                _context.DetallesPedido.Remove(detalle);
                await _context.SaveChangesAsync();
            }
        }*/


    }
}
