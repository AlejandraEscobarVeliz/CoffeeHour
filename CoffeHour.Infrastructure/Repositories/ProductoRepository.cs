// CoffeHour.Infrastructure/Repositories/ProductoRepository.cs
using CoffeHour.Core.Entities;
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeHour.Infrastructure.Repositories
{
    public class ProductoRepository : BaseRepository<Productos>, IProductoRepository
    {
        private readonly DapperContext _dapper;

        public ProductoRepository(CoffeeHourContext context, DapperContext dapper)
            : base(context)
        {
            _dapper = dapper;
        }

       
        public async Task<IEnumerable<Productos>> GetAllDapperAsync()
        {
            var sql = "SELECT * FROM Productos";
            return await _dapper.QueryAsync<Productos>(sql);
        }

    
        public async Task<IEnumerable<Productos>> GetFilteredAsync(string? categoria, string? estado)
        {
            var query = _entities.AsQueryable();

            if (!string.IsNullOrEmpty(categoria))
                query = query.Where(p => p.Categoria == categoria);

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(p => p.Estado == estado);

            return await query.ToListAsync();
        }


    }
}

