using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CoffeHour.Infrastructure.Repositories
{
    public class ProductoRepository : BaseRepository<Productos>, IProductoRepository
    {
    
        private readonly DapperContext _dapper;
        private readonly CoffeeHourContext _context;

        public ProductoRepository(CoffeeHourContext context, DapperContext dapper) : base(context)
        {
            _context = context;
            _dapper = dapper;
        }

        public async Task<IEnumerable<Productos>> GetAllDapperAsync()
        {
            var sql = "SELECT * FROM Productos";
            var productos = await _dapper.QueryAsync<Productos>(sql);
            return productos;
        }

        public IEnumerable<object> GetAllQueryable()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Productos>> GetFilteredAsync(string? categoria, string? estado)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(categoria))
                query = query.Where(p => p.Categoria == categoria);

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(p => p.Estado == estado);

            return await query.ToListAsync();
        }
    }
}

