using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_with_CSHARP_v2.EntityCore.DataAccess;
using ToDo_with_CSHARP_v2.EntityCore.Models;
using ToDo_with_CSHARP_v2.EntityCore.Repository.Interfaces;

namespace ToDo_with_CSHARP_v2.EntityCore.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TaskRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Models.Task>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT t.*, c.*, s.*, p.*
                FROM Tasks t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                INNER JOIN Statuses s ON t.StatusId = s.Id
                INNER JOIN Priorities p ON t.PriorityId = p.Id
                ORDER BY t.Position, t.DueDate";

            var tasks = await connection.QueryAsync<Models.Task, Category, Status, Priority, Models.Task>(
                sql,
                (task, category, status, priority) =>
                {
                    task.Category = category;
                    task.Status = status;
                    task.Priority = priority;
                    return task;
                },
                splitOn: "Id,Id,Id"
            );

            return tasks;
        }

        public async Task<Models.Task> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT t.*, c.*, s.*, p.*
                FROM Tasks t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                INNER JOIN Statuses s ON t.StatusId = s.Id
                INNER JOIN Priorities p ON t.PriorityId = p.Id
                WHERE t.Id = @Id";

            var result = await connection.QueryAsync<Models.Task, Category, Status, Priority, Models.Task>(
                sql,
                (task, category, status, priority) =>
                {
                    task.Category = category;
                    task.Status = status;
                    task.Priority = priority;
                    return task;
                },
                new { Id = id },
                splitOn: "Id,Id,Id"
            );

            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Models.Task task)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                INSERT INTO Tasks (Title, Description, DueDate, CategoryId, StatusId, PriorityId, 
                                   Position, SubTasks, HasDeadlineWarning, IsCompleted, CreatedDate)
                VALUES (@Title, @Description, @DueDate, @CategoryId, @StatusId, @PriorityId, 
                        @Position, @SubTasksJson, @HasDeadlineWarning, @IsCompleted, @CreatedDate);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await connection.ExecuteScalarAsync<int>(sql, task);
        }

        public async Task<bool> UpdateAsync(Models.Task task)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                UPDATE Tasks 
                SET Title = @Title,
                    Description = @Description,
                    DueDate = @DueDate,
                    CategoryId = @CategoryId,
                    StatusId = @StatusId,
                    PriorityId = @PriorityId,
                    Position = @Position,
                    SubTasks = @SubTasksJson,
                    HasDeadlineWarning = @HasDeadlineWarning,
                    IsCompleted = @IsCompleted,
                    CompletedDate = @CompletedDate
                WHERE Id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, task);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = "DELETE FROM Tasks WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> UpdateStatusAsync(int taskId, int statusId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = "UPDATE Tasks SET StatusId = @StatusId WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = taskId, StatusId = statusId });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Models.Task>> SearchAsync(string keyword)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT t.*, c.*, s.*, p.*
                FROM Tasks t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                INNER JOIN Statuses s ON t.StatusId = s.Id
                INNER JOIN Priorities p ON t.PriorityId = p.Id
                WHERE t.Title LIKE @Keyword OR t.Description LIKE @Keyword
                ORDER BY t.Position, t.DueDate";

            var searchKeyword = $"%{keyword}%";

            var tasks = await connection.QueryAsync<Models.Task, Category, Status, Priority, Models.Task>(
                sql,
                (task, category, status, priority) =>
                {
                    task.Category = category;
                    task.Status = status;
                    task.Priority = priority;
                    return task;
                },
                new { Keyword = searchKeyword },
                splitOn: "Id,Id,Id"
            );

            return tasks;
        }

        public async Task<IEnumerable<Models.Task>> GetDueTodayAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT t.*, c.*, s.*, p.*
                FROM Tasks t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                INNER JOIN Statuses s ON t.StatusId = s.Id
                INNER JOIN Priorities p ON t.PriorityId = p.Id
                WHERE CAST(t.DueDate AS DATE) = CAST(GETDATE() AS DATE)
                AND t.IsCompleted = 0";

            var tasks = await connection.QueryAsync<Models.Task, Category, Status, Priority, Models.Task>(
                sql,
                (task, category, status, priority) =>
                {
                    task.Category = category;
                    task.Status = status;
                    task.Priority = priority;
                    return task;
                },
                splitOn: "Id,Id,Id"
            );

            return tasks;
        }

        public async Task<IEnumerable<Models.Task>> GetOverdueAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT t.*, c.*, s.*, p.*
                FROM Tasks t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                INNER JOIN Statuses s ON t.StatusId = s.Id
                INNER JOIN Priorities p ON t.PriorityId = p.Id
                WHERE t.DueDate < GETDATE()
                AND t.IsCompleted = 0
                AND t.DueDate IS NOT NULL";

            var tasks = await connection.QueryAsync<Models.Task, Category, Status, Priority, Models.Task>(
                sql,
                (task, category, status, priority) =>
                {
                    task.Category = category;
                    task.Status = status;
                    task.Priority = priority;
                    return task;
                },
                splitOn: "Id,Id,Id"
            );

            return tasks;
        }

        public async Task<IEnumerable<Models.Task>> GetWithDeadlineWarningAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                SELECT t.*, c.*, s.*, p.*
                FROM Tasks t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                INNER JOIN Statuses s ON t.StatusId = s.Id
                INNER JOIN Priorities p ON t.PriorityId = p.Id
                WHERE t.HasDeadlineWarning = 1
                AND t.IsCompleted = 0";

            var tasks = await connection.QueryAsync<Models.Task, Category, Status, Priority, Models.Task>(
                sql,
                (task, category, status, priority) =>
                {
                    task.Category = category;
                    task.Status = status;
                    task.Priority = priority;
                    return task;
                },
                splitOn: "Id,Id,Id"
            );

            return tasks;
        }

        public async Task<bool> UpdateDeadlineWarningsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = "EXEC UpdateDeadlineWarnings";
            await connection.ExecuteAsync(sql);
            return true;
        }
    }
}
