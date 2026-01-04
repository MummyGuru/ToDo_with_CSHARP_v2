using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo_with_CSHARP_v2.EntityCore.Repository.Interfaces;
using ToDo_with_CSHARP_v2.EntityCore.DataAccess;
using ToDo_with_CSHARP_v2.EntityCore.Models;
using System.Linq;

namespace ToDo_with_CSHARP_v2.EntityCore.Repository
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PriorityRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Priority>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Priorities ORDER BY Level DESC";
            return await connection.QueryAsync<Priority>(sql);
        }

        public async Task<Priority> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Priorities WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Priority>(sql, new { Id = id });
        }

        public async Task<Priority> GetDefaultAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Priorities WHERE IsDefault = 1";
            var result = await connection.QueryAsync<Priority>(sql);
            return result.FirstOrDefault() ?? (await GetAllAsync()).FirstOrDefault();
        }

        public async Task<int> CreateAsync(Priority priority)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                INSERT INTO Priorities (Name, Color, Level, IsDefault)
                VALUES (@Name, @Color, @Level, @IsDefault);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await connection.ExecuteScalarAsync<int>(sql, priority);
        }

        public async Task<bool> UpdateAsync(Priority priority)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                UPDATE Priorities 
                SET Name = @Name,
                    Color = @Color,
                    Level = @Level,
                    IsDefault = @IsDefault
                WHERE Id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, priority);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string checkSql = "SELECT COUNT(*) FROM Tasks WHERE PriorityId = @Id";
            var taskCount = await connection.ExecuteScalarAsync<int>(checkSql, new { Id = id });

            if (taskCount > 0)
                return false;

            const string deleteSql = "DELETE FROM Priorities WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(deleteSql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> SetAsDefaultAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string resetSql = "UPDATE Priorities SET IsDefault = 0";
            await connection.ExecuteAsync(resetSql);

            const string setSql = "UPDATE Priorities SET IsDefault = 1 WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(setSql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<Priority> GetByLevelAsync(int level)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Priorities WHERE Level = @Level";
            return await connection.QueryFirstOrDefaultAsync<Priority>(sql, new { Level = level });
        }

        public async Task<Priority> GetByNameAsync(string name)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Priorities WHERE Name = @Name";
            return await connection.QueryFirstOrDefaultAsync<Priority>(sql, new { Name = name });
        }
    }
}