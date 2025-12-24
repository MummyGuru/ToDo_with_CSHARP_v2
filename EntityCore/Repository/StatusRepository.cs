using Dapper;
using EntityCore.DataAccess;
using EntityCore.Models;
using EntityCore.Repository.Interfaces;

namespace EntityCore.Repository
{
    public class StatusRepository : IStatusRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public StatusRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Statuses ORDER BY Id";
            return await connection.QueryAsync<Status>(sql);
        }

        public async Task<Status> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Statuses WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Status>(sql, new { Id = id });
        }

        public async Task<Status> GetDefaultAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Statuses WHERE IsDefault = 1";
            var result = await connection.QueryAsync<Status>(sql);
            return result.FirstOrDefault() ?? (await GetAllAsync()).FirstOrDefault();
        }

        public async Task<int> CreateAsync(Status status)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                INSERT INTO Statuses (Name, Color, IsDefault)
                VALUES (@Name, @Color, @IsDefault);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await connection.ExecuteScalarAsync<int>(sql, status);
        }

        public async Task<bool> UpdateAsync(Status status)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                UPDATE Statuses 
                SET Name = @Name,
                    Color = @Color,
                    IsDefault = @IsDefault
                WHERE Id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, status);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string checkSql = "SELECT COUNT(*) FROM Tasks WHERE StatusId = @Id";
            var taskCount = await connection.ExecuteScalarAsync<int>(checkSql, new { Id = id });

            if (taskCount > 0)
                return false;

            const string deleteSql = "DELETE FROM Statuses WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(deleteSql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> SetAsDefaultAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string resetSql = "UPDATE Statuses SET IsDefault = 0";
            await connection.ExecuteAsync(resetSql);

            const string setSql = "UPDATE Statuses SET IsDefault = 1 WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(setSql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<Status> GetByNameAsync(string name)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Statuses WHERE Name = @Name";
            return await connection.QueryFirstOrDefaultAsync<Status>(sql, new { Name = name });
        }
    }
}