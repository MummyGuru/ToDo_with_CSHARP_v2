using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo_with_CSHARP_v2.EntityCore.Repository.Interfaces;
using ToDo_with_CSHARP_v2.EntityCore.DataAccess;
using ToDo_with_CSHARP_v2.EntityCore.Models;

namespace ToDo_with_CSHARP_v2.EntityCore.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CommentRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Comment>> GetByTaskIdAsync(int taskId)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Comments WHERE TaskId = @TaskId";
            return await connection.QueryAsync<Comment>(sql, new { TaskId = taskId });
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Comments WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Comment>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(Comment comment)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                INSERT INTO Comments (TaskId, Text)
                VALUES (@TaskId, @Text);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await connection.ExecuteScalarAsync<int>(sql, comment);
        }

        public async Task<bool> UpdateAsync(Comment comment)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                UPDATE Comments 
                SET Text = @Text
                WHERE Id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, comment);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "DELETE FROM Comments WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> DeleteByTaskIdAsync(int taskId)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "DELETE FROM Comments WHERE TaskId = @TaskId";
            var affectedRows = await connection.ExecuteAsync(sql, new { TaskId = taskId });
            return affectedRows > 0;
        }

        public async Task<int> GetCountByTaskIdAsync(int taskId)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT COUNT(*) FROM Comments WHERE TaskId = @TaskId";
            return await connection.ExecuteScalarAsync<int>(sql, new { TaskId = taskId });
        }
    }
}