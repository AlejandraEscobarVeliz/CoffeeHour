using CoffeHour.Core.Entities;

namespace CoffeHour.Core.Interfaces
{
    /// <summary>
    /// Repositorio para operaciones específicas de Clientes.
    /// Los métodos CRUD básicos vienen de IBaseRepository.
    /// </summary>
    public interface IClienteRepository : IBaseRepository<Clientes>
    {
        Task<Clientes?> GetByEmailAsync(string email);
        Task<IEnumerable<Clientes>> GetActiveClientsAsync();
        //Task<IEnumerable<Clientes>> GetAllAsync();
        //Task<Clientes?> GetByIdAsync(int id);
        //Task AddAsync(Clientes cliente);
        //Task UpdateAsync(Clientes cliente);
        //Task DeleteAsync(int id);
    }
}
