using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly CoffeeHourContext _context;

        public ProductoRepository(CoffeeHourContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Productos>> GetAllAsync() =>
            await _context.Productos.ToListAsync();

        public async Task<Productos?> GetByIdAsync(int id) =>
            await _context.Productos.FindAsync(id);

        public async Task AddAsync(Productos producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Productos producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
