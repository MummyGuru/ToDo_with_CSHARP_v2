using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo_with_CSHARP_v2.EntityCore.Repository.Interfaces;
using ToDo_with_CSHARP_v2.EntityCore.DataAccess;
using ToDo_with_CSHARP_v2.EntityCore.Models;
using System.Linq;

namespace ToDo_with_CSHARP_v2.EntityCore.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CategoryRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Categories ORDER BY Position";
            return await connection.QueryAsync<Category>(sql);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Categories WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
        }

        public async Task<Category> GetDefaultAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Categories WHERE IsDefault = 1";
            var result = await connection.QueryAsync<Category>(sql);
            return result.FirstOrDefault();
        }
    }
}
