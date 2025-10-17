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
    public class ClienteRepository : IClienteRepository
    {
        private readonly CoffeeHourContext _context;

        public ClienteRepository(CoffeeHourContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Clientes>> GetAllAsync() =>
            await _context.Clientes.ToListAsync();

        public async Task<Clientes?> GetByIdAsync(int id) =>
            await _context.Clientes.FindAsync(id);

        public async Task AddAsync(Clientes cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Clientes cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
