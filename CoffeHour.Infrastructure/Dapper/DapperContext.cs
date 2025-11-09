using Dapper;
using Microsoft.Extensions.Configuration;
//using MySql.Data.MySqlClient;
using MySqlConnector;
using System.Data;

namespace CoffeHour.Infrastructure.Data
{
    /// <summary>
    /// Contexto de conexión Dapper para ejecutar consultas SQL directas.
    /// </summary>
    public class DapperContext
    {
        private readonly IDbConnection _connection;


        private readonly IConfiguration _configuration;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /*public DapperContext(IConfiguration configuration)
        {
            _connection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }*/

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, param);
            }
        }

    }

}


